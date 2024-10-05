using Domain.Classes;
using Domain.Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Classes
{
    /// <summary>
    /// Service implementation for ITaskService
    /// </summary>
    public class TaskService : ITaskService
    {
        // Repositories
        private ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Retrieves all tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of all tasks as <see cref="TasksDto"/> objects
        /// </returns>
        public List<TasksDto> AllTasks(string userName, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userName))
                return new List<TasksDto>();

            // gets result
            var result = _taskRepository.AllTasks(userName, isAdmin);

            // returns converted result
            return result.Select(task => ConvertToTasksDto(task)).ToList();
        }

        /// <summary>
        /// Retrieves pending tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose pending tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of pending tasks as <see cref="TasksDto"/> objects
        /// </returns>
        public List<TasksDto> Pendings(string userName, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userName))
                return new List<TasksDto>();

            // gets result
            var result = _taskRepository.Pendings(userName, isAdmin);

            // returns converted result
            return result.Select(task => ConvertToTasksDto(task)).ToList();
        }

        /// <summary>
        /// Retrieves completed tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose completed tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of completed tasks as <see cref="TasksDto"/> objects
        /// </returns>
        public List<TasksDto> Completed(string userName, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userName))
                return new List<TasksDto>();

            // gets result
            var result = _taskRepository.Completed(userName, isAdmin);

            // returns converted result
            return result.Select(task => ConvertToTasksDto(task)).ToList();
        }

        /// <summary>
        /// Retrieves high priority tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose high priority tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of high priority tasks as <see cref="TasksDto"/> objects
        /// </returns>
        public List<TasksDto> HighPriority(string userName, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userName))
                return new List<TasksDto>();

            // gets result
            var result = _taskRepository.HighPriority(userName, isAdmin);

            // returns converted result
            return result.Select(task => ConvertToTasksDto(task)).ToList();
        }

        /// <summary>
        /// Retrieves medium priority tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose medium priority tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of medium priority tasks as <see cref="TasksDto"/> objects
        /// </returns>
        public List<TasksDto> MediumPriority(string userName, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userName))
                return new List<TasksDto>();

            // gets result
            var result = _taskRepository.MediumPriority(userName, isAdmin);

            // returns converted result
            return result.Select(task => ConvertToTasksDto(task)).ToList();
        }

        /// <summary>
        /// Retrieves low priority tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose low priority tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of low priority tasks as <see cref="TasksDto"/> objects
        /// </returns>
        public List<TasksDto> LowPriority(string userName, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userName))
                return new List<TasksDto>();

            // gets result
            var result = _taskRepository.LowPriority(userName, isAdmin);

            // returns converted result
            return result.Select(task => ConvertToTasksDto(task)).ToList();
        }

        /// <summary>
        /// Retrieves all available task categories
        /// </summary>
        /// <returns>
        /// List of task categories as <see cref="Category"/> objects
        /// </returns>
        public List<CategoryDto> Categories()
        {
            // gets result
            var result = _taskRepository.Categories();

            // returns converted result
            return result.Select(category => ConvertToCategoryDto(category)).ToList();
        }

        /// <summary>
        /// Adds a new task to the system
        /// </summary>
        /// <param name="data">The task data as a <see cref="TasksDto"/> object</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        public async Task<Result> SaveTaskAsync(TasksDto data)
        {
            // conversion
            var task = new Tasks()
            {
                TaskTitle      = data.TaskTitle ?? string.Empty,
                Deadline       = data.Deadline,
                TaskNote       = data.TaskNote ?? string.Empty,
                HighPriority   = data.HighPriority,
                MediumPriority = data.MediumPriority,
                LowPriority    = data.LowPriority,
                CatId          = data.CatId,
                UserId         = data.UserId,
                Pending        = data.Pending
            };

            // returns result
            return await _taskRepository.SaveTaskAsync(task);
        }

        /// <summary>
        /// Retrieves detailed information about a specific task
        /// </summary>
        /// <param name="taskId">The ID of the task to retrieve information for</param>
        /// <returns>
        /// List of task details as <see cref="TasksDto"/> objects
        /// </returns>
        public List<TasksDto> TaskInfo(int taskId)
        {
            if (taskId <= 0)
                return new List<TasksDto>();

            // gets result
            var result = _taskRepository.TaskInfo(taskId);

            // returns converted result
            return result.Select(info => ConvertToTasksDto(info)).ToList();
        }

        /// <summary>
        /// Edits an existing task in the system
        /// </summary>
        /// <param name="data">The task data as a <see cref="TasksDto"/> object</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        public async Task<Result> EditTaskAsync(TasksDto data)
        {
            // conversion
            var task = new Tasks()
            {
                TaskId         = data.TaskId,
                TaskTitle      = data.TaskTitle ?? string.Empty,
                Deadline       = data.Deadline,
                TaskNote       = data.TaskNote ?? string.Empty,
                HighPriority   = data.HighPriority,
                MediumPriority = data.MediumPriority,
                LowPriority    = data.LowPriority,
                CatId          = data.CatId,
                UserId         = data.UserId
            };

            // returns result
            return await _taskRepository.EditTaskAsync(task);
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
            // returns result
            return await _taskRepository.DeleteTaskAsync(taskId);
        }

        /// <summary>
        /// Adds a note to an existing task
        /// </summary>
        /// <param name="data">The task data as a <see cref="TasksDto"/> object containing the note</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        public async Task<Result> AddTaskNoteAsync(TasksDto data)
        {
            // conversion
            var task = new Tasks()
            {
                TaskId   = data.TaskId,
                UserNote = data.UserNote
            };

            // returns result
            return await _taskRepository.AddTaskNoteAsync(task);
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
            // returns result
            return await _taskRepository.MarkasDone(taskId);
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
            if (string.IsNullOrEmpty(userName))
                return new List<TaskCount>();

            // returns result
            return _taskRepository.TasksCount(userName, isAdmin);
        }

        // Helpers -----------------------------------------------------

        /// <summary>
        /// Converts a single <see cref="Tasks"/> entity to a <see cref="TasksDto"/> object. Maps all 
        /// relevant properties from the <see cref="Tasks"/> entity to the corresponding 
        /// properties in <see cref="TasksDto"/>
        /// </summary>
        /// <param name="task">The <see cref="Tasks"/> entity to be converted</param>
        /// <returns>
        /// A <see cref="TasksDto"/> object containing the mapped data
        /// </returns>
        private TasksDto ConvertToTasksDto(Tasks task)
        {
            if (task is null)
                return new TasksDto();

            // returns converted list
            return new TasksDto
            {
                TaskId         = task.TaskId,
                TaskTitle      = task.TaskTitle,
                Deadline       = task.Deadline,
                TaskNote       = task.TaskNote,
                Pending        = task.Pending,
                Complete       = task.Complete,
                HighPriority   = task.HighPriority,
                MediumPriority = task.MediumPriority,
                LowPriority    = task.LowPriority,
                UserNote       = task.UserNote,
                UserName       = task.Users.UserName,
                FirstName      = task.Users.FirstName,
                UserMail       = task.Users.UserMail,
                CatId          = task.CatId,
                CatName        = task.Category.CatName,
                UserId         = task.UserId
            };
        }

        /// <summary>
        /// Converts a single <see cref="Category"/> entity to a <see cref="CategoryDto"/> object. Maps all 
        /// relevant properties from the <see cref="Category"/> entity to the corresponding 
        /// properties in <see cref="CategoryDto"/>
        /// </summary>
        /// <param name="task">The <see cref="Category"/> entity to be converted</param>
        /// <returns>
        /// A <see cref="CategoryDto"/> object containing the mapped data
        /// </returns>
        private CategoryDto ConvertToCategoryDto(Category category)
        {
            if (category is null)
                return new CategoryDto();

            // returns converted list
            return new CategoryDto
            {
                CatId   = category.CatId,
                CatName = category.CatName
            };
        }
    }
}
