using System.Threading.Tasks;
using DataLibrary.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserInterface.Models;
using System.Security.Claims;
using DataLibrary.Models;

namespace UserInterface.ViewComponents
{
    public class NavigateViewComponent : ViewComponent
    {
        // services
        private readonly IDataService _dataService;
        private UserManager<IdentityUser> _userManager { get; }
        public NavigateViewComponent(IDataService dataService, UserManager<IdentityUser> userManager)
        {
            _dataService = dataService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // get sign in user name
            var user = await _userManager.GetUserAsync((ClaimsPrincipal)User);
            var userName = user.UserName;

            // checking user role is admin or not
            ClaimsPrincipal currentUser = (ClaimsPrincipal)User;
            var role = currentUser.IsInRole(Roles.lead);

            var viewModel = new LayoutViewModel
            {
                user_name = userName,
                role = role,
                _taskcount = _dataService.TaskCount(userName, role)
            };

            return View(viewModel);
        }
    }
}