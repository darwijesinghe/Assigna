using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DataLibrary.Interfaces;
using DataLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserInterface.Models;
using UserInterface.Services;
using static UserInterface.Models.TaskViewModel;

namespace UserInterface.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        // services
        private readonly IDataService _dataService;
        private UserManager<IdentityUser> _userManager { get; }
        public IMailService _mailSend { get; }
        private SignInManager<IdentityUser> _signinManager { get; }
        public TaskController(IDataService dataService, SignInManager<IdentityUser> signinManager,
        UserManager<IdentityUser> userManager, IMailService mailSend)
        {
            _dataService = dataService;
            _signinManager = signinManager;
            _userManager = userManager;
            _mailSend = mailSend;
        }

        // landing action
        [HttpGet]
        public async Task<IActionResult> Tasks()
        {
            var viewModel = await TasksList("All");
            return View(viewModel);
        }

        // pending task
        [HttpGet]
        public async Task<IActionResult> Pending()
        {
            var viewModel = await TasksList("Pending");
            return View(viewModel);
        }

        // completed task
        [HttpGet]
        public async Task<IActionResult> Complete()
        {
            var viewModel = await TasksList("Complete");
            return View(viewModel);
        }

        // high priority task
        [HttpGet("h-priority")]
        public async Task<IActionResult> HighPriority()
        {
            var viewModel = await TasksList("High");
            return View(viewModel);
        }

        // medium priority task
        [HttpGet("m-priority")]
        public async Task<IActionResult> MediumPriority()
        {
            var viewModel = await TasksList("Medium");
            return View(viewModel);
        }

        // low priority task
        [HttpGet("l-priority")]
        public async Task<IActionResult> LowPriority()
        {
            var viewModel = await TasksList("Low");
            return View(viewModel);
        }

        // use single method to return all list
        private async Task<TaskViewModel> TasksList(string type)
        {
            // get user id
            var user = await _userManager.GetUserAsync(User);
            var userName = user.UserName;

            // checking user role is admin or not
            ClaimsPrincipal currentUser = this.User;
            var role = currentUser.IsInRole(Roles.lead);

            TaskViewModel viewModel = new();

            switch (type)
            {
                case "All":
                    viewModel = new TaskViewModel()
                    {
                        _tasks = _dataService.AllTasks(userName, role),
                    };
                    break;
                case "Pending":
                    viewModel = new TaskViewModel()
                    {
                        _pending = _dataService.Pendings(userName, role),
                    };
                    break;
                case "Complete":
                    viewModel = new TaskViewModel()
                    {
                        _complete = _dataService.Completed(userName, role),
                    };
                    break;
                case "High":
                    viewModel = new TaskViewModel()
                    {
                        _hpriority = _dataService.HighPriority(userName, role),
                    };
                    break;
                case "Medium":
                    viewModel = new TaskViewModel()
                    {
                        _mpriority = _dataService.MediumPriority(userName, role),
                    };
                    break;
                case "Low":
                    viewModel = new TaskViewModel()
                    {
                        _lpriority = _dataService.LowPriority(userName, role),
                    };
                    break;
            }

            return viewModel;

        }

        // use single method to get task count for ajax requests
        private async Task<TaskCount> TaskCount()
        {
            // get user id
            var user = await _userManager.GetUserAsync(User);
            var userName = user.UserName;

            // checking user role is admin or not
            ClaimsPrincipal currentUser = this.User;
            var role = currentUser.IsInRole(Roles.lead);

            var data = _dataService.TaskCount(userName, role)[0];
            return new TaskCount
            {
                all_task = data.all_task,
                pendings = data.pendings,
                complete = data.complete,
                high = data.high,
                medium = data.medium,
                low = data.low
            };
        }

        // new task
        [HttpGet("new-task")]
        public IActionResult NewTask()
        {
            var viewModel = new NewTaskViewModel()
            {
                _category = _dataService.Categories(),
                _users = _dataService.Users()
            };

            return View(viewModel);
        }

        // add a new task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddTask(NewTaskViewModel data)
        {
            // send data to save a new task
            var task = new TaskDto()
            {
                tsk_title = data.tsk_title,
                deadline = Convert.ToDateTime(data.deadline),
                tsk_note = data.tsk_note,
                prio_high = (data.priority == "High") ? true : false,
                prio_medium = (data.priority == "Medium") ? true : false,
                prio_low = (data.priority == "Low") ? true : false,
                cat_id = data.tsk_category,
                user_id = data.assign_to,
                pending = true
            };

            var result = await _dataService.SaveTaskAsync(task);
            if (result.success)
            {
                // task count
                var count = this.TaskCount();

                return Json(new
                {
                    message = "New task was added successfully",
                    success = true,
                    tasks = count
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

        // task infomtaion
        [HttpGet("task-info")]
        public IActionResult TaskInfo(int taskId)
        {
            var viewModel = new TaskViewModel()
            {
                _taskinfo = _dataService.TaskInfo(taskId)
            };

            return View(viewModel);
        }

        // edit a task
        [HttpGet("edit-task")]
        public IActionResult EditTask(int taskId)
        {
            // filter request task
            var task = _dataService.TaskInfo(taskId)[0];

            // return 404 if task completed
            if (task.complete)
            {
                return NotFound();
            }

            var viewModel = new NewTaskViewModel()
            {
                tsk_id = task.tsk_id,
                tsk_title = task.tsk_title ?? string.Empty,
                tsk_category = task.cat_id,
                deadline = task.deadline.ToString("yyyy-MM-dd"),
                priority = (task.prio_high) ? "High" : (task.prio_medium) ? "Medium" : "Low",
                assign_to = task.user_id,
                tsk_note = task.tsk_note ?? string.Empty,
                _category = _dataService.Categories(),
                _users = _dataService.Users()
            };

            return View(viewModel);
        }

        // edit task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditTask(NewTaskViewModel data)
        {

            // send data to save a new task
            var task = new TaskDto()
            {
                tsk_id = data.tsk_id,
                tsk_title = data.tsk_title,
                deadline = Convert.ToDateTime(data.deadline),
                tsk_note = data.tsk_note,
                prio_high = (data.priority == "High") ? true : false,
                prio_medium = (data.priority == "Medium") ? true : false,
                prio_low = (data.priority == "Low") ? true : false,
                cat_id = data.tsk_category,
                user_id = data.assign_to
            };

            var result = await _dataService.EditTaskAsync(task);
            if (result.success)
            {
                // task count
                var count = this.TaskCount();

                return Json(new
                {
                    message = "Task was updated successfully",
                    success = true,
                    tasks = count
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

        // delete a task
        [HttpPost]
        public async Task<JsonResult> DeleteTask(int taskId)
        {

            var result = await _dataService.DeleteTaskAsync(taskId);
            if (result.success)
            {
                var html = "<span class='no_task'>No task information was found, Maybe it was deleted</span><a href='/tasks' class='back_tasks link_button'>Back to Tasks</a>";

                // task count
                var count = this.TaskCount();

                return Json(new
                {
                    message = "Task was deleted successfully",
                    success = true,
                    result = html,
                    tasks = count
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

        // add a note
        [HttpGet("add-note")]
        public IActionResult AddNote(int taskId)
        {
            var viewModel = new AddNoteViewModel()
            {
                _taskinfo = _dataService.TaskInfo(taskId)
            };

            return View(viewModel);
        }

        // write a task note
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> WriteNote(AddNoteViewModel data)
        {
            // send data to add task note
            var task = new TaskDto()
            {
                tsk_id = data.tsk_id,
                user_note = data.user_note
            };

            var result = await _dataService.AddTaskNoteAsync(task);
            if (result.success)
            {

                return Json(new
                {
                    message = "Your note was added successfully",
                    success = true,
                    result = data.user_note
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

        // mark as done
        [HttpPost]
        public async Task<JsonResult> MarkasDone(int taskId)
        {

            var result = await _dataService.MarkasDone(taskId);
            if (result.success)
            {
                // task count
                var count = this.TaskCount();

                return Json(new
                {
                    message = "Task was marked as completed",
                    success = true,
                    result = "Completed",
                    tasks = count
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

        // send reminder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SendReminder(RemindViewModel data)
        {
            // get user email
            var mail = _dataService.TaskInfo(data.task_id)[0].user_mail;
            if (mail is not null)
            {
                // mail subject
                string subject = "Assigna task reminder";

                // mail body
                string body = data.message ?? string.Empty;

                var result = await _mailSend.SendMailAsync(mail, subject, body);
                if (result.success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Remind sent successfully"
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
}