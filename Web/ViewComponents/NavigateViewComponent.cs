using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserInterface.Models;
using System.Security.Claims;
using Services.Interfaces;
using Domain.Classes;

namespace UserInterface.ViewComponents
{
    /// <summary>
    /// Navigation component class
    /// </summary>
    public class NavigateViewComponent : ViewComponent
    {
        // Services
        private readonly ITaskService     _taskService;
        private UserManager<IdentityUser> _userManager;

        public NavigateViewComponent(ITaskService taskService, UserManager<IdentityUser> userManager)
        {
            _taskService = taskService;
            _userManager = userManager;
        }

        /// <summary>
        /// Invokes the view component, rendering its content
        /// </summary>
        /// <returns>
        /// A <see cref="IViewComponentResult"/> that represents the result of the view component invocation
        /// </returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // gets sign in user name
            var user     = await _userManager.GetUserAsync((ClaimsPrincipal)User);
            var userName = user.UserName;

            // checking user role is admin or not
            var currentUser = (ClaimsPrincipal)User;
            var role        = currentUser.IsInRole(Roles.lead);

            // view data
            var viewModel = new LayoutViewModel
            {
                UserName  = userName,
                Role      = role,
                TaskCount = _taskService.TasksCount(userName, role)
            };

            // returns result
            return View(viewModel);
        }
    }
}