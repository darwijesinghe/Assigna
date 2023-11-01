using DataLibrary.Interfaces;
using DataLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserInterface.Models;
using UserInterface.Services;
using static UserInterface.Models.ResetViewModel;

namespace UserInterface.Controllers
{
    public class AccountController : Controller
    {
        // services
        private UserManager<IdentityUser> UserManager { get; }
        private SignInManager<IdentityUser> SigninManager { get; }
        private IMailService MailSend { get; }
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDataService _dataService;

        [TempData]
        public string? AlertMessage { get; set; }

        public AccountController(IDataService dataService, SignInManager<IdentityUser> signinManager,
        UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IMailService mailSend)
        {
            _dataService = dataService;
            SigninManager = signinManager;
            UserManager = userManager;
            _roleManager = roleManager;
            MailSend = mailSend;
        }

        // sign in landing action
        [HttpGet]
        public async Task<IActionResult> SignIn()
        {
            // workaroud for remove return url
            // clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (!string.IsNullOrEmpty(Request.QueryString.Value))
            {
                return RedirectToAction("SignIn");
            }

            return View(new SignInViewModel());
        }

        // local sign in
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SignIn(SignInViewModel data)
        {
            // check existing user
            string userName = data.UserName;
            string password = data.Password;
            var exist = await UserManager.FindByNameAsync(userName);
            if (exist is null)
            {
                return Json(new
                {
                    success = false,
                    message = "User name or password is incorrect."
                });

            }
            else
            {
                var result = await SigninManager.PasswordSignInAsync(userName, password, false, false);
                if (result.Succeeded)
                {

                    return Json(new
                    {
                        success = true,
                        message = "Ok",
                        returnurl = "/tasks"
                    });

                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "User name or password is incorrect."
                    });
                }
            }
        }

        // external signin
        [HttpPost, Route("oauth/{provider}.{type?}")]
        public IActionResult Oauth([FromRoute] string provider, [FromRoute] string? type)
        {
            // login provider
            string oauth = string.Empty;

            // user type / role
            string role = type ?? "no-role-provided";

            AuthenticationProperties properties = new();

            switch (provider)
            {
                case "Google":
                    oauth = provider;

                    // redirect url
                    string redirectUrl = Url.Action("google", "account", new { type = role });

                    properties = SigninManager.ConfigureExternalAuthenticationProperties(oauth, redirectUrl);
                    break;

            }

            return new ChallengeResult(oauth, properties);
        }

        // external sigin response
        [HttpGet]
        public async Task<IActionResult> Google(string type)
        {
            // user role
            string role = type;

            // get external login infomation
            var info = await SigninManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                // return error to user
                AlertMessage = "Sign in information not found.";
                return LocalRedirect("/");
            }

            // sign in user with the provider if user already has a account
            // otherwise create an account and add role
            var result = await SigninManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);
            if (result.Succeeded)
            {
                // return user to task page on success
                return LocalRedirect("/tasks");
            }
            else
            {

                // get user mail address
                string mail = info.Principal.FindFirst(ClaimTypes.Email).Value;
                if (mail is null)
                {
                    // no external login info -> cookie expire or provider issue
                    AlertMessage = "We could not find your information.";
                    return LocalRedirect("/");
                }

                // create account if user not exist
                var user = await UserManager.FindByEmailAsync(mail);
                if (user is null)
                {
                    // return user when no role provided -> this happens in sign in mode
                    // we need a user role type to assign to the new user. That can capture from the URl route when the user is signing up

                    if (role != Roles.member & role != Roles.lead)
                    {
                        AlertMessage = "Account type does not match with correct user type. Please sign up first.";
                        return LocalRedirect("/");
                    }

                    user = new IdentityUser
                    {
                        UserName = info.Principal.FindFirst(ClaimTypes.GivenName).Value,
                        Email = info.Principal.FindFirst(ClaimTypes.Email).Value
                    };

                    var create = await UserManager.CreateAsync(user);
                    if (!create.Succeeded)
                    {
                        AlertMessage = "Something went wrong, Please try again later.";
                        return LocalRedirect("/");
                    }

                    // create role
                    var userRole = _roleManager.FindByNameAsync(role).Result;
                    if (userRole != null)
                    {
                        await UserManager.AddToRoleAsync(user, userRole.Name);
                    }
                    else
                    {
                        var newRole = new IdentityRole
                        {
                            Name = role
                        };

                        await _roleManager.CreateAsync(newRole);
                        await UserManager.AddToRoleAsync(user, newRole.Name);
                    }

                    // add user to extra user detail table
                    var adduser = new UsersDto()
                    {
                        UserName = user.UserName,
                        FirstName = user.UserName,
                        UserMail = user.Email,
                        IsAdmin = (role == Roles.lead)
                    };

                    await _dataService.SaveNewUserAsync(adduser);
                }

                // add login for new or existing user
                var login = await UserManager.AddLoginAsync(user, info);
                if (login.Succeeded)
                {
                    // log in user to application
                    await SigninManager.SignInAsync(user, false);
                    return LocalRedirect("/tasks");
                }
                else
                {
                    AlertMessage = "Error occurred during the login process.";
                    return LocalRedirect("/");
                }
            }
        }

        // user sign out
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await SigninManager.SignOutAsync();
            return LocalRedirect("/");
        }

        // sign up landing action
        [HttpGet]
        public IActionResult SignUp(string type)
        {
            // send user type
            var viewmodel = new SignUpViewModel()
            {
                Role = type
            };

            return View(viewmodel);
        }

        // signup action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SignUp(SignUpViewModel data)
        {
            // user role not accepted
            if (data.Role != Roles.member & data.Role != Roles.lead)
            {
                return Json(new
                {
                    success = false,
                    message = "The account type is not accepted."
                });
            }

            // check existing emails
            string email = data.Email;
            var exist = await UserManager.FindByEmailAsync(email);
            if (exist is not null)
            {
                return Json(new
                {
                    success = false,
                    message = "Email address already exists."
                });
            }

            // new user
            var user = new IdentityUser
            {
                UserName = data.UserName,
                Email = data.Email
            };

            // user password
            string password = data.Password;

            // role
            string role = data.Role;

            // create new user
            var result = await UserManager.CreateAsync(user: user, password: password);
            if (result.Succeeded)
            {
                // create role
                var userRole = _roleManager.FindByNameAsync(role).Result;
                if (userRole != null)
                {
                    await UserManager.AddToRoleAsync(user, userRole.Name);
                }
                else
                {
                    var newRole = new IdentityRole
                    {
                        Name = role
                    };

                    await _roleManager.CreateAsync(newRole);
                    await UserManager.AddToRoleAsync(user, newRole.Name);
                }

                // add user to extra user detail table
                var adduser = new UsersDto()
                {
                    UserName = data.UserName,
                    FirstName = data.FirstName,
                    UserMail = user.Email,
                    IsAdmin = (role == Roles.lead)
                };

                await _dataService.SaveNewUserAsync(adduser);

                return Json(new
                {
                    success = true,
                    message = "Ok",
                    returnurl = "/"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong, Please try again later."
                });
            }
        }

        // forgot password
        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View(new ResetViewModel.SendResetLink());
        }

        // send password reset link
        [HttpPost, ActionName("forgot-password")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ForgotPassword(SendResetLink data)
        {
            // check existing user
            string mail = data.Mail;
            var user = await UserManager.FindByEmailAsync(mail);
            if (user is null)
            {
                return Json(new
                {
                    success = false,
                    message = "Email not found to send reset link."
                });
            }
            else
            {
                // generate token
                var token = await UserManager.GeneratePasswordResetTokenAsync(user);

                // callback url
                var callback = Url.Action("reset-password", "account", new { token, email = user.Email }, Request.Scheme);

                // mail subject
                string subject = "Assigna password reset link.";

                // mail body
                string body = $"Please click the link below to reset your assigna account password. <br> <a href='{callback}'>Click here to reset the password.</a>";

                var result = await MailSend.SendMailAsync(mail, subject, body);
                if (result.Success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Recovery email sent. If you don’t see this email in your inbox within 15 minutes, look for it in your spam folder. If you find it there, please mark it as not spam."
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Something went wrong, Please try again later."
                    });
                }
            }
        }

        // reset password
        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetViewModel() { Token = token, Mail = email });
        }

        // reset password to new one
        [HttpPost, ActionName("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ResetPassword(ResetViewModel data)
        {
            // check existing user
            string email = data.Mail;
            string token = data.Token;
            string password = data.Password;
            var user = await UserManager.FindByEmailAsync(email);
            if (user is null)
            {
                return Json(new
                {
                    success = false,
                    message = "Could not find account details."
                });
            }
            else
            {
                var result = await UserManager.ResetPasswordAsync(user, token, password);
                if (result.Succeeded)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Password reset successful. Your password has been reset to the new one. Please click the below link to sign in to your assigna account."
                    });
                }
                else
                {
                    return Json(new
                    {
                        message = "Something went wrong, Please try again later.",
                        success = false
                    });
                }
            }
        }

    }
}
