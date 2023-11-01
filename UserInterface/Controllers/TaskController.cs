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
        private UserManager<IdentityUser> UserManager { get; }
        public IMailService MailSend { get; }
        public TaskController(IDataService dataService, UserManager<IdentityUser> userManager, IMailService mailSend)
        {
            _dataService = dataService;
            UserManager = userManager;
            MailSend = mailSend;
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
            var user = await UserManager.GetUserAsync(User);
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
                        Tasks = _dataService.AllTasks(userName, role),
                    };
                    break;
                case "Pending":
                    viewModel = new TaskViewModel()
                    {
                        Pending = _dataService.Pendings(userName, role),
                    };
                    break;
                case "Complete":
                    viewModel = new TaskViewModel()
                    {
                        Complete = _dataService.Completed(userName, role),
                    };
                    break;
                case "High":
                    viewModel = new TaskViewModel()
                    {
                        Hpriority = _dataService.HighPriority(userName, role),
                    };
                    break;
                case "Medium":
                    viewModel = new TaskViewModel()
                    {
                        Mpriority = _dataService.MediumPriority(userName, role),
                    };
                    break;
                case "Low":
                    viewModel = new TaskViewModel()
                    {
                        Lpriority = _dataService.LowPriority(userName, role),
                    };
                    break;
            }

            return viewModel;

        }

        // use single method to get task count for ajax requests
        private async Task<TaskCount> TaskCount()
        {
            // get user id
            var user = await UserManager.GetUserAsync(User);
            var userName = user.UserName;

            // checking user role is admin or not
            ClaimsPrincipal currentUser = this.User;
            var role = currentUser.IsInRole(Roles.lead);

            var data = _dataService.TaskCount(userName, role)[0];
            return new TaskCount
            {
                AllTask = data.AllTask,
                Pendings = data.Pendings,
                Complete = data.Complete,
                High = data.High,
                Medium = data.Medium,
                Low = data.Low
            };
        }

        // new task
        [HttpGet("new-task")]
        public IActionResult NewTask()
        {
            var viewModel = new NewTaskViewModel()
            {
                Category = _dataService.Categories(),
                Users = _dataService.Users()
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
                TskTitle = data.TskTitle,
                Deadline = Convert.ToDateTime(data.Deadline),
                TskNote = data.TskNote,
                PrioHigh = (data.Priority == "High"),
                PrioMedium = (data.Priority == "Medium"),
                PrioLow = (data.Priority == "Low"),
                CatId = data.TskCategory,
                UserId = data.AssignTo,
                Pending = true
            };

            var result = await _dataService.SaveTaskAsync(task);
            if (result.Success)
            {
                // task count
                var count = this.TaskCount();

                return Json(new
                {
                    message = "New task was added successfully.",
                    success = true,
                    tasks = count
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

        // task infomtaion
        [HttpGet("task-info")]
        public IActionResult TaskInfo(int taskId)
        {
            var viewModel = new TaskViewModel()
            {
                Taskinfo = _dataService.TaskInfo(taskId)
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
            if (task.Complete)
            {
                return NotFound();
            }

            var viewModel = new NewTaskViewModel()
            {
                TskId = task.TskId,
                TskTitle = task.TskTitle ?? string.Empty,
                TskCategory = task.CatId,
                Deadline = task.Deadline.ToString("yyyy-MM-dd"),
                Priority = (task.PrioHigh) ? "High" : (task.PrioMedium) ? "Medium" : "Low",
                AssignTo = task.UserId,
                TskNote = task.TskNote ?? string.Empty,
                Category = _dataService.Categories(),
                Users = _dataService.Users()
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
                TskId = data.TskId,
                TskTitle = data.TskTitle,
                Deadline = Convert.ToDateTime(data.Deadline),
                TskNote = data.TskNote,
                PrioHigh = (data.Priority == "High"),
                PrioMedium = (data.Priority == "Medium"),
                PrioLow = (data.Priority == "Low"),
                CatId = data.TskCategory,
                UserId = data.AssignTo
            };

            var result = await _dataService.EditTaskAsync(task);
            if (result.Success)
            {
                // task count
                var count = this.TaskCount();

                return Json(new
                {
                    message = "Task was updated successfully.",
                    success = true,
                    tasks = count
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

        // delete a task
        [HttpPost]
        public async Task<JsonResult> DeleteTask(int taskId)
        {

            var result = await _dataService.DeleteTaskAsync(taskId);
            if (result.Success)
            {
                var html = "<span class='no_task'>No task information was found, Maybe it was deleted.</span><a href='/tasks' class='back_tasks link_button'>Back to Tasks</a>";

                // task count
                var count = this.TaskCount();

                return Json(new
                {
                    message = "Task was deleted successfully.",
                    success = true,
                    result = html,
                    tasks = count
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

        // add a note
        [HttpGet("add-note")]
        public IActionResult AddNote(int taskId)
        {
            var viewModel = new AddNoteViewModel()
            {
                Taskinfo = _dataService.TaskInfo(taskId)
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
                TskId = data.TskId,
                UserNote = data.UserNote
            };

            var result = await _dataService.AddTaskNoteAsync(task);
            if (result.Success)
            {

                return Json(new
                {
                    message = "Your note was added successfully.",
                    success = true,
                    result = data.UserNote
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

        // mark as done
        [HttpPost]
        public async Task<JsonResult> MarkasDone(int taskId)
        {

            var result = await _dataService.MarkasDone(taskId);
            if (result.Success)
            {
                // task count
                var count = this.TaskCount();

                return Json(new
                {
                    message = "Task was marked as completed.",
                    success = true,
                    result = "Completed",
                    tasks = count
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

        // send reminder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SendReminder(RemindViewModel data)
        {
            // get user email
            var mail = _dataService.TaskInfo(data.TaskId)[0].UserMail;
            if (mail is not null)
            {
                // mail subject
                string subject = "Assigna task reminder";

                // mail body
                string body = data.Message ?? string.Empty;

                var result = await MailSend.SendMailAsync(mail, subject, body);
                if (result.Success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Remind sent successfully."
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
}