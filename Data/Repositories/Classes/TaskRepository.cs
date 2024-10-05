using Domain.Classes;
using Domain.Data;
using Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repositories.Classes
{
    /// <summary>
    /// Repository implementation for task operations
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        // Data context
        private DataContext _context { get; }

        public TaskRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of all tasks as <see cref="Tasks"/> objects
        /// </returns>
        public List<Tasks> AllTasks(string userName, bool isAdmin)
        {
            // gets all tasks
            var taks = MakeTaskList();

            // if user is an admin the return full list
            // otherwise filter list by user id/name

            if (isAdmin)
            {
                return taks.OrderBy(x => x.TaskId).ToList();
            }

            return taks.Where(x => x.Users.UserName == userName)
                       .OrderBy(x => x.TaskId)
                       .ToList();
        }

        /// <summary>
        /// Retrieves pending tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose pending tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of pending tasks as <see cref="Tasks"/> objects
        /// </returns>
        public List<Tasks> Pendings(string userName, bool isAdmin)
        {
            // gets all tasks
            var taks = MakeTaskList();

            // if user is an admin the return full list
            // otherwise filter list by user id

            if (isAdmin)
            {
                return taks.Where(x => x.Pending)
                           .OrderBy(x => x.TaskId)
                           .ToList();
            }

            return taks.Where(x => x.Pending && x.Users.UserName == userName)
                       .OrderBy(x => x.TaskId)
                       .ToList();
        }

        /// <summary>
        /// Retrieves completed tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose completed tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of completed tasks as <see cref="Tasks"/> objects
        /// </returns>
        public List<Tasks> Completed(string userName, bool isAdmin)
        {
            // gets all tasks
            var taks = MakeTaskList();

            // if user is an admin the return full list
            // otherwise filter list by user id

            if (isAdmin)
            {
                return taks.Where(x => x.Complete)
                           .OrderBy(x => x.TaskId)
                           .ToList();
            }

            return taks.Where(x => x.Complete && x.Users.UserName == userName)
                       .OrderBy(x => x.TaskId)
                       .ToList();
        }

        /// <summary>
        /// Retrieves high priority tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose high priority tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of high priority tasks as <see cref="Tasks"/> objects
        /// </returns>
        public List<Tasks> HighPriority(string userName, bool isAdmin)
        {
            // gets all tasks
            var taks = MakeTaskList();

            // if user is an admin the return full list
            // otherwise filter list by user id

            if (isAdmin)
            {
                return taks.Where(x => x.HighPriority)
                           .OrderBy(x => x.TaskId)
                           .ToList();
            }

            return taks.Where(x => x.HighPriority && x.Users.UserName == userName)
                       .OrderBy(x => x.TaskId)
                       .ToList();
        }

        /// <summary>
        /// Retrieves medium priority tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose medium priority tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of medium priority tasks as <see cref="Tasks"/> objects
        /// </returns>
        public List<Tasks> MediumPriority(string userName, bool isAdmin)
        {
            // gets all tasks
            var taks = MakeTaskList();

            // if user is an admin the return full list
            // otherwise filter list by user id

            if (isAdmin)
            {
                return taks.Where(x => x.MediumPriority)
                           .OrderBy(x => x.TaskId)
                           .ToList();
            }

            return taks.Where(x => x.MediumPriority && x.Users.UserName == userName)
                       .OrderBy(x => x.TaskId)
                       .ToList();
        }

        /// <summary>
        /// Retrieves low priority tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose low priority tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of low priority tasks as <see cref="Tasks"/> objects
        /// </returns>
        public List<Tasks> LowPriority(string userName, bool isAdmin)
        {
            // gets all tasks
            var taks = MakeTaskList();

            // if user is an admin the return full list
            // otherwise filter list by user id

            if (isAdmin)
            {
                return taks.Where(x => x.LowPriority)
                           .OrderBy(x => x.TaskId)
                           .ToList();
            }

            return taks.Where(x => x.LowPriority && x.Users.UserName == userName)
                       .OrderBy(x => x.TaskId)
                       .ToList();
        }

        /// <summary>
        /// Retrieves all available task categories
        /// </summary>
        /// <returns>
        /// List of task categories as <see cref="Category"/> objects
        /// </returns>
        public List<Category> Categories()
        {
            // gets categories
            var categories = _context.Category
                                     .Select(x => new Category
                                     {
                                         CatId = x.CatId,
                                         CatName = x.CatName
                                     })
                                    .OrderBy(x => x.CatId)
                                    .ToList();

            if (categories == null || !categories.Any())
                return new List<Category>();

            // returns result
            return categories;
        }

        /// <summary>
        /// Adds a new task to the system
        /// </summary>
        /// <param name="data">The task data as a <see cref="Tasks"/> object</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        public async Task<Result> SaveTaskAsync(Tasks data)
        {
            // adds the tasks entity to the context
            await _context.Tasks.AddAsync(data);

            try
            {
                await _context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Retrieves detailed information about a specific task
        /// </summary>
        /// <param name="taskId">The ID of the task to retrieve information for</param>
        /// <returns>
        /// List of task details as <see cref="Tasks"/> objects
        /// </returns>
        public List<Tasks> TaskInfo(int taskId)
        {
            // gets task info
            var taks = MakeTaskList();

            // returns result
            return taks.Where(x => x.TaskId == taskId).OrderBy(x => x.TaskId).ToList();
        }

        /// <summary>
        /// Edits an existing task in the system
        /// </summary>
        /// <param name="data">The task data as a <see cref="Tasks"/> object</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        public async Task<Result> EditTaskAsync(Tasks data)
        {
            // gets task
            var task = _context.Tasks.FirstOrDefault(x => x.TaskId == data.TaskId);

            if (task is null)
                return new Result { Message = "No task found to update.", Success = false };

            // assigns new values
            task.TaskTitle      = data.TaskTitle ?? string.Empty;
            task.CatId          = data.CatId;
            task.Deadline       = data.Deadline;
            task.HighPriority   = data.HighPriority;
            task.MediumPriority = data.MediumPriority;
            task.LowPriority    = data.LowPriority;
            task.UserId         = data.UserId;
            task.TaskNote       = data.TaskNote ?? string.Empty;

            try
            {
                await _context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Deletes a task from the system by its ID
        /// </summary>
        /// <param name="taskId">The ID of the task to delete</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        public async Task<Result> DeleteTaskAsync(int taskId)
        {
            // gets task
            var task = _context.Tasks.FirstOrDefault(x => x.TaskId == taskId);

            if (task is null)
                return new Result { Message = "No task found to update.", Success = false };

            _context.Tasks.Remove(task);

            try
            {
                await _context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok",
                    Success = true,
                    Id = taskId
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Adds a note to an existing task
        /// </summary>
        /// <param name="data">The task data as a <see cref="Tasks"/> object containing the note</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        public async Task<Result> AddTaskNoteAsync(Tasks data)
        {
            // gets task
            var task = _context.Tasks.FirstOrDefault(x => x.TaskId == data.TaskId);

            if (task is null)
                return new Result { Message = "No task found to update.", Success = false };

            // assigns new values
            task.TaskNote = data.UserNote ?? string.Empty;

            try
            {
                await _context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Marks a task as completed by its ID
        /// </summary>
        /// <param name="taskId">The ID of the task to mark as done</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        public async Task<Result> MarkasDone(int taskId)
        {
            // gets task
            var task = _context.Tasks.FirstOrDefault(x => x.TaskId == taskId);

            if (task is null)
                return new Result { Message = "No task found to update.", Success = false };

            // assigns new values
            task.Pending  = false;
            task.Complete = true;

            try
            {
                await _context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Retrieves the count of tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose task count to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of task counts as <see cref="TaskCount"/> objects
        /// </returns>
        public List<TaskCount> TasksCount(string userName, bool isAdmin)
        {
            // tempory lists
            var tasks      = new List<Tasks>();
            var tasksCount = new List<TaskCount>();

            // if admin
            if (isAdmin)
            {
                // all tasks
                tasks = _context.Tasks.ToList();
            }

            // if member
            else
            {
                // task list for a user
                tasks = (from us in _context.Users
                         join tk in _context.Tasks on us.UserId equals tk.Users.UserId
                         select new Tasks
                         {
                             TaskId = tk.TaskId,
                             Pending = tk.Pending,
                             Complete = tk.Complete,
                             HighPriority = tk.HighPriority,
                             MediumPriority = tk.MediumPriority,
                             LowPriority = tk.LowPriority,

                             Users = new Users
                             {
                                 UserName = us.UserName,
                                 FirstName = us.FirstName,
                                 UserMail = us.UserMail
                             }
                         })
                        .Where(x => x.Users.UserName == userName)
                        .ToList();

            }

            // task count
            var task = new TaskCount
            {
                AllTask  = tasks.Count(),
                Pendings = tasks.Where(x => x.Pending == true).Count(),
                Complete = tasks.Where(x => x.Complete == true).Count(),
                High     = tasks.Where(x => x.HighPriority == true).Count(),
                Medium   = tasks.Where(x => x.MediumPriority == true).Count(),
                Low      = tasks.Where(x => x.LowPriority == true).Count()
            };

            // adds to the list
            tasksCount.Add(task);

            // returns result
            return tasksCount.ToList();
        }

        /// <summary>
        /// Creates and returns a list of task objects. This method generates a 
        /// list of <see cref="Tasks"/> based on specific criteria or input data
        /// </summary>
        /// <returns>
        /// A list of <see cref="Tasks"/> objects
        /// </returns>
        private List<Tasks> MakeTaskList()
        {
            // gets all tasks
            var tasks = (from us in _context.Users
                         join tk in _context.Tasks on us.UserId equals tk.Users.UserId
                         select new Tasks
                         {
                             TaskId         = tk.TaskId,
                             TaskTitle      = tk.TaskTitle,
                             Deadline       = tk.Deadline,
                             TaskNote       = tk.TaskNote,
                             Pending        = tk.Pending,
                             Complete       = tk.Complete,
                             HighPriority   = tk.HighPriority,
                             MediumPriority = tk.MediumPriority,
                             LowPriority    = tk.LowPriority,
                             UserNote       = tk.TaskNote,
                             CatId          = tk.CatId,
                             UserId         = tk.UserId,

                             Category = new Category
                             {
                                 CatId   = tk.CatId,
                                 CatName = tk.Category.CatName
                             },

                             Users = new Users
                             {
                                 UserName  = us.UserName,
                                 FirstName = us.FirstName,
                                 UserMail  = us.UserMail
                             }

                         }).ToList();

            if (tasks == null || !tasks.Any())
                return new List<Tasks>();

            // returns result
            return tasks;
        }
    }
}
