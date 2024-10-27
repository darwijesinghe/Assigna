using Domain.Classes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using UserInterface.Models;
using static UserInterface.Models.ResetViewModel;

namespace UserInterface.Controllers
{
    public class AccountController : Controller
    {
        // Services
        private UserManager<IdentityUser>          _userManager;
        private SignInManager<IdentityUser>        _signinManager;
        private IMailService                       _mailSend;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService              _userService;

        [TempData]
        public string? AlertMessage { get; set; }

        public AccountController(IUserService userService, SignInManager<IdentityUser> signinManager,
        UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IMailService mailSend)
        {
            _userService   = userService;
            _signinManager = signinManager;
            _userManager   = userManager;
            _roleManager   = roleManager;
            _mailSend      = mailSend;
        }

        /// <summary>
        /// Handles the sign-in process for a user.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the sign-in operation, 
        /// which may include a redirect or view result.
        /// </returns>
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

        /// <summary>
        /// Processes the sign-in request using the provided sign-in data.
        /// </summary>
        /// <param name="data">The sign-in information encapsulated in a <see cref="SignInViewModel"/>.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> indicating the outcome of the sign-in operation, 
        /// which may include success or error messages.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SignIn(SignInViewModel data)
        {
            // checks existing user
            string userName = data.UserName;
            string password = data.Password;
            var exist       = await _userManager.FindByNameAsync(userName);

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
                // signin user
                var result = await _signinManager.PasswordSignInAsync(userName, password, false, false);
                if (result.Succeeded)
                {

                    return Json(new
                    {
                        success   = true,
                        message   = "Ok.",
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

        /// <summary>
        /// Handles OAuth authentication based on the specified provider and type.
        /// </summary>
        /// <param name="provider">The OAuth provider (e.g., Google, Facebook, etc.) specified in the route.</param>
        /// <param name="type">An optional string representing the type of OAuth operation, if applicable.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the OAuth authentication or operation.
        /// </returns>
        [HttpPost, Route("oauth/{provider}.{type?}")]
        public IActionResult Oauth([FromRoute] string provider, [FromRoute] string? type)
        {
            // login provider
            string oauth = string.Empty;

            // user type / role
            string role = type ?? "no-role-provided";

            // auth properties
            var properties = new AuthenticationProperties();

            switch (provider)
            {
                case "Google":
                    oauth = provider;

                    // redirect url
                    string redirectUrl = Url.Action("google", "account", new { type = role });
                    properties         = _signinManager.ConfigureExternalAuthenticationProperties(oauth, redirectUrl);
                    break;

            }

            // returns challenge result
            return new ChallengeResult(oauth, properties);
        }

        /// <summary>
        /// Handles Google authentication or operations based on the specified type.
        /// </summary>
        /// <param name="type">A string representing the type of Google operation (e.g., sign-in, authentication, etc.).</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the outcome of the Google operation.
        /// </returns>
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
                AlertMessage = "Sign in information not found.";
                return LocalRedirect("/");
            }

            // sign in user with the provider if user already has an account
            // otherwise create an account and add role
            var result = await _signinManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);
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
                var user = await _userManager.FindByEmailAsync(mail);
                if (user is null)
                {
                    // return user when no role provided -> this happens in sign in mode
                    // we need a user role type to assign to the new user. That can capture from the URl route when the user is signing up

                    if (role != Roles.member & role != Roles.lead)
                    {
                        AlertMessage = "Account type does not match with correct account type.";
                        return LocalRedirect("/");
                    }

                    user = new IdentityUser
                    {
                        UserName = info.Principal.FindFirst(ClaimTypes.GivenName).Value,
                        Email = info.Principal.FindFirst(ClaimTypes.Email).Value
                    };

                    // creates user
                    var create = await _userManager.CreateAsync(user);
                    if (!create.Succeeded)
                    {
                        AlertMessage = "Something went wrong, Please try again later.";
                        return LocalRedirect("/");
                    }

                    // creates role
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

                    // adds user to extra user detail table
                    var adduser = new UsersDto()
                    {
                        UserName  = user.UserName,
                        FirstName = user.UserName,
                        UserMail  = user.Email,
                        IsAdmin   = (role == Roles.lead)
                    };

                    // saves to extra table
                    await _userService.SaveNewUserAsync(adduser);
                }

                // add login for new or existing user
                var login = await _userManager.AddLoginAsync(user, info);
                if (login.Succeeded)
                {
                    // login user to application
                    await _signinManager.SignInAsync(user, false);
                    return LocalRedirect("/tasks");
                }
                else
                {
                    AlertMessage = "Error occurred during the login process.";
                    return LocalRedirect("/");
                }
            }
        }

        /// <summary>
        /// Handles the sign-out process for the user.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the sign-out operation, 
        /// typically a redirect to a login page or homepage.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signinManager.SignOutAsync();
            return LocalRedirect("/");
        }

        /// <summary>
        /// Handles the sign-up process based on the specified type.
        /// </summary>
        /// <param name="type">A string representing the type of user role (e.g. team-lead).</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the sign-up process.
        /// </returns>
        [HttpGet]
        public IActionResult SignUp(string type)
        {
            // view data
            var viewmodel = new SignUpViewModel()
            {
                Role = type
            };

            // returns result
            return View(viewmodel);
        }

        /// <summary>
        /// Processes the user sign-up request using the provided sign-up data.
        /// </summary>
        /// <param name="data">The sign-up information encapsulated in a <see cref="SignUpViewModel"/>.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> indicating the outcome of the sign-up process, 
        /// which may include success or error messages.
        /// </returns>
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

            // checks existing emails
            string email = data.Email;
            var exist    = await _userManager.FindByEmailAsync(email);

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
                Email    = data.Email
            };

            // user password
            string password = data.Password;

            // role
            string role = data.Role;

            // creates new user
            var result = await _userManager.CreateAsync(user: user, password: password);
            if (result.Succeeded)
            {
                // creates role
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

                // user details
                var adduser = new UsersDto()
                {
                    UserName  = data.UserName,
                    FirstName = data.FirstName,
                    UserMail  = user.Email,
                    IsAdmin   = (role == Roles.lead)
                };

                // adds user to extra user detail table
                await _userService.SaveNewUserAsync(adduser);

                return Json(new
                {
                    success   = true,
                    message   = "Ok.",
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

        /// <summary>
        /// Displays the forgot password page or initiates the forgot password process.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the forgot password page or handles related logic.
        /// </returns>
        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            // returns result
            return View(new SendResetLink());
        }

        /// <summary>
        /// Processes the request to send a password reset link to the user's email.
        /// </summary>
        /// <param name="data">The reset link information encapsulated in a <see cref="SendResetLink"/> object.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> indicating the result of the operation, 
        /// such as success or failure messages.
        /// </returns>
        [HttpPost, ActionName("forgot-password")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ForgotPassword(SendResetLink data)
        {
            // checks existing user
            string mail = data.Mail;
            var user    = await _userManager.FindByEmailAsync(mail);

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
                // generates token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // callback url
                var callback = Url.Action("reset-password", "account", new { token, email = user.Email }, Request.Scheme);

                // mail subject
                string subject = "Assigna password reset link.";

                // mail body
                string body = $"Please click the link below to reset your assigna account password. <br> <a href='{callback}'>Click here to reset the password.</a>";

                // sends an email to the user
                var result = await _mailSend.SendMailAsync(mail, subject, body);
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

        /// <summary>
        /// Displays the password reset page or initiates the password reset process using the provided token and email.
        /// </summary>
        /// <param name="token">The password reset token sent to the user.</param>
        /// <param name="email">The user's email address for which the password reset is being performed.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the reset password page or handles the reset process.
        /// </returns>
        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string token, string email)
        {
            // returns result
            return View(new ResetViewModel() { Token = token, Mail = email });
        }

        /// <summary>
        /// Processes the password reset request using the provided reset data.
        /// </summary>
        /// <param name="data">The password reset information encapsulated in a <see cref="ResetViewModel"/> object.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> indicating the outcome of the password reset process, 
        /// such as success or failure messages.
        /// </returns>
        [HttpPost, ActionName("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ResetPassword(ResetViewModel data)
        {
            // checks existing user
            string email    = data.Mail;
            string token    = data.Token;
            string password = data.Password;

            var user = await _userManager.FindByEmailAsync(email);
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
                // resets password
                var result = await _userManager.ResetPasswordAsync(user, token, password);
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
