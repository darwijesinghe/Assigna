using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Classes;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using UserInterface.Models;
using UserInterface.Services;
using static UserInterface.Models.TaskViewModel;

namespace UserInterface.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        // Services
        private UserManager<IdentityUser>          _userManager;
        private SignInManager<IdentityUser>        _signinManager;
        private IMailService                       _mailSend;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITaskService              _taskService;
        private readonly IUserService              _userService;

        public TaskController(ITaskService taskService, IUserService userService, SignInManager<IdentityUser> signinManager,
        UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IMailService mailSend)
        {
            _taskService   = taskService;
            _userService   = userService;
            _signinManager = signinManager;
            _userManager   = userManager;
            _roleManager   = roleManager;
            _mailSend      = mailSend;
        }

        /// <summary>
        /// Retrieves and displays the list of tasks for the user
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the tasks view with the user's task data
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Tasks()
        {
            // view data
            var data = await TasksList(TaskType.All);
            return View(data);
        }

        /// <summary>
        /// Retrieves and displays the pending list of tasks for the user
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the tasks view with the user's task data
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Pending()
        {
            // view data
            var data = await TasksList(TaskType.Pending);
            return View(data);
        }

        /// <summary>
        /// Retrieves and displays the completed list of tasks for the user
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the tasks view with the user's task data
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Complete()
        {
            // view data
            var data = await TasksList(TaskType.Complete);
            return View(data);
        }

        /// <summary>
        /// Retrieves and displays the high priority list of tasks for the user
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the tasks view with the user's task data
        /// </returns>
        [HttpGet("h-priority")]
        public async Task<IActionResult> HighPriority()
        {
            // view data
            var data = await TasksList(TaskType.High);
            return View(data);
        }

        /// <summary>
        /// Retrieves and displays the medium priority list of tasks for the user
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the tasks view with the user's task data
        /// </returns>
        [HttpGet("m-priority")]
        public async Task<IActionResult> MediumPriority()
        {
            // view data
            var data = await TasksList(TaskType.Medium);
            return View(data);
        }

        /// <summary>
        /// Retrieves and displays the low priority list of tasks for the user
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the tasks view with the user's task data
        /// </returns>
        [HttpGet("l-priority")]
        public async Task<IActionResult> LowPriority()
        {
            // view data
            var data = await TasksList(TaskType.Low);
            return View(data);
        }

        /// <summary>
        /// Displays the page for creating a new task
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the new task creation view
        /// </returns>
        [Authorize(Roles = Roles.lead)]
        [HttpGet("new-task")]
        public IActionResult NewTask()
        {
            // view data
            var data = new NewTaskViewModel()
            {
                Category = _taskService.Categories(),
                Users    = _userService.AllUsers()
            };

            return View(data);
        }

        /// <summary>
        /// Processes the request to add a new task using the provided task data
        /// </summary>
        /// <param name="data">The new task information encapsulated in a <see cref="NewTaskViewModel"/> object</param>
        /// <returns>
        /// A <see cref="JsonResult"/> indicating the outcome of the task addition process, 
        /// such as success or failure messages
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddTask(NewTaskViewModel data)
        {
            // new task data
            var task = new TasksDto()
            {
                TaskTitle      = data.TaskTitle,
                Deadline       = Convert.ToDateTime(data.Deadline),
                TaskNote       = data.TaskNote,
                HighPriority   = (data.Priority == TaskType.High),
                MediumPriority = (data.Priority == TaskType.Medium),
                LowPriority    = (data.Priority == TaskType.Low),
                CatId          = data.CatId,
                UserId         = data.AssignTo,
                Pending        = true
            };

            // creates new task
            var result = await _taskService.SaveTaskAsync(task);
            if (result.Success)
            {
                // task count
                var count = this.TasksCount();

                return Json(new
                {
                    message = "New task was added successfully.",
                    success = true,
                    tasks   = count
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

        /// <summary>
        /// Retrieves and displays detailed information about a specific task
        /// </summary>
        /// <param name="taskId">The unique identifier of the task whose information is to be retrieved</param>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the task information view
        /// </returns>
        [HttpGet("task-info")]
        public IActionResult TaskInfo(int taskId)
        {
            // view data
            var data = new TaskViewModel()
            {
                TaskInfo = _taskService.TaskInfo(taskId)
            };

            return View(data);
        }

        /// <summary>
        /// Displays the page for editing a specific task based on its unique identifier
        /// </summary>
        /// <param name="taskId">The unique identifier of the task to be edited</param>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the task editing view
        /// </returns>
        [HttpGet("edit-task")]
        public IActionResult EditTask(int taskId)
        {
            // filter request task
            var task = _taskService.TaskInfo(taskId)[0];

            // return 404 if task completed
            if (task.Complete)
            {
                return NotFound();
            }

            // view data
            var data = new NewTaskViewModel()
            {
                TaskId    = task.TaskId,
                TaskTitle = task.TaskTitle ?? string.Empty,
                CatId     = task.CatId,
                Deadline  = task.Deadline.ToString("yyyy-MM-dd"),
                Priority  = (task.HighPriority) ? TaskType.High : (task.MediumPriority) ? TaskType.Medium : TaskType.Low,
                AssignTo  = task.UserId,
                TaskNote  = task.TaskNote ?? string.Empty,
                Category  = _taskService.Categories(),
                Users     = _userService.AllUsers()
            };

            return View(data);
        }

        /// <summary>
        /// Processes the request to update an existing task with the provided data
        /// </summary>
        /// <param name="data">The updated task information encapsulated in a <see cref="NewTaskViewModel"/> object</param>
        /// <returns>
        /// A <see cref="JsonResult"/> indicating the outcome of the task update process, 
        /// such as success or failure messages
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditTask(NewTaskViewModel data)
        {
            // edit task data
            var task = new TasksDto()
            {
                TaskId         = data.TaskId,
                TaskTitle      = data.TaskTitle,
                Deadline       = Convert.ToDateTime(data.Deadline),
                TaskNote       = data.TaskNote,
                HighPriority   = (data.Priority == TaskType.High),
                MediumPriority = (data.Priority == TaskType.Medium),
                LowPriority    = (data.Priority == TaskType.Low),
                CatId          = data.CatId,
                UserId         = data.AssignTo
            };

            // edits task data
            var result = await _taskService.EditTaskAsync(task);
            if (result.Success)
            {
                // task count
                var count = this.TasksCount();

                return Json(new
                {
                    message = "Task was updated successfully.",
                    success = true,
                    tasks   = count
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

        /// <summary>
        /// Processes the request to delete a task identified by its unique identifier
        /// </summary>
        /// <param name="taskId">The unique identifier of the task to be deleted</param>
        /// <returns>
        /// A <see cref="JsonResult"/> indicating the outcome of the task deletion process, 
        /// such as success or failure messages
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteTask(int taskId)
        {
            // deletes task
            var result = await _taskService.DeleteTaskAsync(taskId);
            if (result.Success)
            {
                // info message
                var html = "<span class='no_task'>No task information was found, Maybe it was deleted.</span><a href='/tasks' class='back_tasks link_button'>Back to Tasks</a>";

                // task count
                var count = this.TasksCount();

                return Json(new
                {
                    message = "Task was deleted successfully.",
                    success = true,
                    result  = html,
                    tasks   = count
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

        /// <summary>
        /// Displays the page for adding a note to a specific task identified by its unique identifier
        /// </summary>
        /// <param name="taskId">The unique identifier of the task to which the note will be added</param>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the note addition view for the specified task
        /// </returns>
        [HttpGet("add-note")]
        public IActionResult AddNote(int taskId)
        {
            // view data
            var data = new AddNoteViewModel()
            {
                TaskId   = taskId,
                Taskinfo = _taskService.TaskInfo(taskId)
            };

            return View(data);
        }

        /// <summary>
        /// Processes the request to add a note to a specific task using the provided note data
        /// </summary>
        /// <param name="data">The note information encapsulated in an <see cref="AddNoteViewModel"/> object</param>
        /// <returns>
        /// A <see cref="JsonResult"/> indicating the outcome of the note addition process, 
        /// such as success or failure messages
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> WriteNote(AddNoteViewModel data)
        {
            // task note data
            var task = new TasksDto()
            {
                TaskId   = data.TaskId,
                UserNote = data.UserNote
            };

            // adds task note
            var result = await _taskService.AddTaskNoteAsync(task);
            if (result.Success)
            {
                return Json(new
                {
                    message = "Your note was added successfully.",
                    success = true,
                    result  = data.UserNote
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

        /// <summary>
        /// Processes the request to mark a specific task as completed based on its unique identifier
        /// </summary>
        /// <param name="taskId">The unique identifier of the task to be marked as done</param>
        /// <returns>
        /// A <see cref="JsonResult"/> indicating the outcome of the task completion process, 
        /// such as success or failure messages
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> MarkasDone(int taskId)
        {
            // marks task as done
            var result = await _taskService.MarkasDone(taskId);
            if (result.Success)
            {
                // task count
                var count = this.TasksCount();

                return Json(new
                {
                    message = "Task was marked as completed.",
                    success = true,
                    result  = "Completed",
                    tasks   = count
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

        /// <summary>
        /// Processes the request to send a reminder based on the provided reminder data
        /// </summary>
        /// <param name="data">The reminder information encapsulated in a <see cref="RemindViewModel"/> object</param>
        /// <returns>
        /// A <see cref="JsonResult"/> indicating the outcome of the reminder sending process, 
        /// such as success or failure messages
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SendReminder(RemindViewModel data)
        {
            // gets user email
            var mail = _taskService.TaskInfo(data.TaskId).FirstOrDefault()?.UserMail;
            if (mail is not null)
            {
                // mail subject
                string subject = "Assigna task reminder";

                // mail body
                string body = data.Message ?? string.Empty;

                // sends email
                var result = await _mailSend.SendMailAsync(mail, subject, body);
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

        // Helpers ---------------------------------------------------------

        /// <summary>
        /// Asynchronously retrieves a list of tasks based on the specified task type
        /// </summary>
        /// <param name="type">The type of tasks to retrieve, specified by the <see cref="TaskType"/> enum</param>
        /// <returns>
        /// A <see cref="TaskViewModel"/> containing the list of tasks of the specified type
        /// </returns>
        private async Task<TaskViewModel> TasksList(TaskType type)
        {
            // gets user id/name
            var user        = await _userManager.GetUserAsync(User);
            var userName    = user.UserName;

            // checking user role is admin or not
            var currentUser = this.User;
            var role        = currentUser.IsInRole(Roles.lead);

            // view data
            var viewModel = new TaskViewModel();

            switch (type)
            {
                case TaskType.All     :
                    viewModel.Tasks = _taskService.AllTasks(userName, role);
                    break;

                case TaskType.Pending :
                    viewModel.Pending = _taskService.Pendings(userName, role);
                    break;

                case TaskType.Complete:
                    viewModel.Complete = _taskService.Completed(userName, role);
                    break;

                case TaskType.High    :
                    viewModel.HighPriority = _taskService.HighPriority(userName, role);
                    break;

                case TaskType.Medium  :
                    viewModel.MediumPriority = _taskService.MediumPriority(userName, role);
                    break;

                case TaskType.Low     :
                    viewModel.LowPriority = _taskService.LowPriority(userName, role);
                    break;
            }

            return viewModel;
        }

        /// <summary>
        /// Retrieves the count of tasks for use in AJAX requests
        /// </summary>
        /// <returns>
        /// A <see cref="TaskCount"/> object containing the total number of tasks
        /// </returns>
        private async Task<TaskCount> TasksCount()
        {
            // gets user id/name
            var user        = await _userManager.GetUserAsync(User);
            var userName    = user.UserName;

            // checking user role is admin or not
            var currentUser = this.User;
            var role        = currentUser.IsInRole(Roles.lead);

            // gets task count
            var data = _taskService.TasksCount(userName, role)[0];
            return new TaskCount
            {
                AllTask  = data.AllTask,
                Pendings = data.Pendings,
                Complete = data.Complete,
                High     = data.High,
                Medium   = data.Medium,
                Low      = data.Low
            };
        }
    }
}