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
        private UserManager<IdentityUser> _userManager { get; }
        private SignInManager<IdentityUser> _signinManager { get; }
        private IMailService _mailSend { get; }
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDataService _dataService;

        [TempData]
        public string? alertMessage { get; set; }

        public AccountController(IDataService dataService, SignInManager<IdentityUser> signinManager,
        UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IMailService mailSend)
        {
            _dataService = dataService;
            _signinManager = signinManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _mailSend = mailSend;
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
            string userName = data.user_name;
            string password = data.password;
            var exist = await _userManager.FindByNameAsync(userName);
            if (exist is null)
            {
                return Json(new
                {
                    success = false,
                    message = "User name or password is incorrect"
                });

            }
            else
            {
                var result = await _signinManager.PasswordSignInAsync(userName, password, false, false);
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
                        message = "User name or password is incorrect"
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

                    properties = _signinManager.ConfigureExternalAuthenticationProperties(oauth, redirectUrl);
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
            var info = await _signinManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                // return error to user
                alertMessage = "Sign in information not found";
                return LocalRedirect("/");
            }

            // sign in user with the provider if user already has a account
            // otherwise create an account and add role
            var result = await _signinManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);
            if (result.Succeeded)
            {
                string userName = info.Principal.FindFirst(ClaimTypes.GivenName).Value.ToLower();
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
                    alertMessage = "We could not find your information";
                    return LocalRedirect("/");
                }

                // create account if user not exist
                var user = await _userManager.FindByEmailAsync(mail);
                if (user is null)
                {
                    // return user when no role provided -> this happens in sign in mode
                    // we need a user role type to assign to the new user. That can capture from the URl route when the user is signing up

                    if (role != Roles.member & role != Roles.lead)
                    {
                        alertMessage = "Account type does not match with correct user type, Please sign up first";
                        return LocalRedirect("/");
                    }

                    user = new IdentityUser
                    {
                        UserName = info.Principal.FindFirst(ClaimTypes.GivenName).Value,
                        Email = info.Principal.FindFirst(ClaimTypes.Email).Value
                    };

                    var create = await _userManager.CreateAsync(user);
                    if (!create.Succeeded)
                    {
                        alertMessage = "Something went wrong, Please try again later";
                        return LocalRedirect("/");
                    }

                    // create role
                    var userRole = _roleManager.FindByNameAsync(role).Result;
                    if (userRole != null)
                    {
                        await _userManager.AddToRoleAsync(user, userRole.Name);
                    }
                    else
                    {
                        var newRole = new IdentityRole
                        {
                            Name = role
                        };

                        await _roleManager.CreateAsync(newRole);
                        await _userManager.AddToRoleAsync(user, newRole.Name);
                    }

                    // add user to extra user detail table
                    var adduser = new UsersDto()
                    {
                        user_name = user.UserName,
                        first_name = user.UserName,
                        user_mail = user.Email,
                        is_admin = (role == Roles.lead) ? true : false
                    };

                    await _dataService.SaveNewUserAsync(adduser);
                }

                // add login for new or existing user
                var login = await _userManager.AddLoginAsync(user, info);
                if (login.Succeeded)
                {
                    // log in user to application
                    await _signinManager.SignInAsync(user, false);
                    return LocalRedirect("/tasks");
                }
                else
                {
                    alertMessage = "Error occurred during the login process";
                    return LocalRedirect("/");
                }
            }
        }

        // user sign out
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signinManager.SignOutAsync();
            return LocalRedirect("/");
        }

        // sign up landing action
        [HttpGet]
        public IActionResult SignUp(string type)
        {
            // send user type
            var viewmodel = new SignUpViewModel()
            {
                role = type
            };

            return View(viewmodel);
        }

        // signup action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SignUp(SignUpViewModel data)
        {
            // user role not accepted
            if (data.role != Roles.member & data.role != Roles.lead)
            {
                return Json(new
                {
                    success = false,
                    message = "The account type is not accepted"
                });
            }

            // check existing emails
            string email = data.email;
            var exist = await _userManager.FindByEmailAsync(email);
            if (exist is not null)
            {
                return Json(new
                {
                    success = false,
                    message = "Email address already exists"
                });
            }

            // new user
            var user = new IdentityUser
            {
                UserName = data.user_name,
                Email = data.email
            };

            // user password
            string password = data.password;

            // role
            string role = data.role;

            // create new user
            var result = await _userManager.CreateAsync(user: user, password: password);
            if (result.Succeeded)
            {
                // create role
                var userRole = _roleManager.FindByNameAsync(role).Result;
                if (userRole != null)
                {
                    await _userManager.AddToRoleAsync(user, userRole.Name);
                }
                else
                {
                    var newRole = new IdentityRole
                    {
                        Name = role
                    };

                    await _roleManager.CreateAsync(newRole);
                    await _userManager.AddToRoleAsync(user, newRole.Name);
                }

                // add user to extra user detail table
                var adduser = new UsersDto()
                {
                    user_name = data.user_name,
                    first_name = data.first_name,
                    user_mail = user.Email,
                    is_admin = (role == Roles.lead) ? true : false
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
                    message = "Something went wrong, Please try again later"
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
            string mail = data.mail;
            var user = await _userManager.FindByEmailAsync(mail);
            if (user is null)
            {
                return Json(new
                {
                    success = false,
                    message = "Email not found to send reset link"
                });
            }
            else
            {
                // generate token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // callback url
                var callback = Url.Action("reset-password", "account", new { token = token, email = user.Email }, Request.Scheme);

                // mail subject
                string subject = "Assigna password reset link";

                // mail body
                string body = $"Please click the link below to reset your shopper account password. <br> <a href='{callback}'>Click here to reset the password.</a>";

                var result = await _mailSend.SendMailAsync(mail, subject, body);
                if (result.success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Recovery email sent. If you don’t see this email in your inbox within 15 minutes, look for it in your spam folder. If you find it there, please mark it as spam"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Something went wrong, Please try again later"
                    });
                }
            }
        }

        // reset password
        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetViewModel() { token = token, mail = email });
        }

        // reset password to new one
        [HttpPost, ActionName("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ResetPassword(ResetViewModel data)
        {
            // check existing user
            string email = data.mail;
            string token = data.token;
            string password = data.password;
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return Json(new
                {
                    success = false,
                    message = "Could not find account details"
                });
            }
            else
            {
                var result = await _userManager.ResetPasswordAsync(user, token, password);
                if (result.Succeeded)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Password reset successful. Your password has been reset to the new one. Please click the below link to sign in to your assigna account"
                    });
                }
                else
                {
                    return Json(new
                    {
                        message = "Something went wrong, Please try again later",
                        success = false
                    });
                }
            }
        }

    }
}
