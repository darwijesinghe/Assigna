using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Classes;

namespace Domain.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for task related operations
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Retrieves all tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of all tasks as <see cref="Tasks"/> objects
        /// </returns>
        List<Tasks> AllTasks(string userName, bool isAdmin);
    
        /// <summary>
        /// Retrieves pending tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose pending tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of pending tasks as <see cref="Tasks"/> objects
        /// </returns>
        List<Tasks> Pendings(string userName, bool isAdmin);

        /// <summary>
        /// Retrieves completed tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose completed tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of completed tasks as <see cref="Tasks"/> objects
        /// </returns>
        List<Tasks> Completed(string userName, bool isAdmin);

        /// <summary>
        /// Retrieves high priority tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose high priority tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of high priority tasks as <see cref="Tasks"/> objects
        /// </returns>
        List<Tasks> HighPriority(string userName, bool isAdmin);

        /// <summary>
        /// Retrieves medium priority tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose medium priority tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of medium priority tasks as <see cref="Tasks"/> objects
        /// </returns>
        List<Tasks> MediumPriority(string userName, bool isAdmin);

        /// <summary>
        /// Retrieves low priority tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose low priority tasks to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of low priority tasks as <see cref="Tasks"/> objects
        /// </returns>
        List<Tasks> LowPriority(string userName, bool isAdmin);

        /// <summary>
        /// Retrieves all available task categories
        /// </summary>
        /// <returns>
        /// List of task categories as <see cref="Category"/> objects
        /// </returns>
        List<Category> Categories();

        /// <summary>
        /// Adds a new task to the system
        /// </summary>
        /// <param name="data">The task data as a <see cref="Tasks"/> object</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        Task<Result> SaveTaskAsync(Tasks data);

        /// <summary>
        /// Retrieves detailed information about a specific task
        /// </summary>
        /// <param name="taskId">The ID of the task to retrieve information for</param>
        /// <returns>
        /// List of task details as <see cref="Tasks"/> objects
        /// </returns>
        List<Tasks> TaskInfo(int taskId);

        /// <summary>
        /// Edits an existing task in the system
        /// </summary>
        /// <param name="data">The task data as a <see cref="Tasks"/> object</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        Task<Result> EditTaskAsync(Tasks data);

        /// <summary>
        /// Deletes a task from the system by its ID
        /// </summary>
        /// <param name="taskId">The ID of the task to delete</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        Task<Result> DeleteTaskAsync(int taskId);

        /// <summary>
        /// Adds a note to an existing task
        /// </summary>
        /// <param name="data">The task data as a <see cref="Tasks"/> object containing the note</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        Task<Result> AddTaskNoteAsync(Tasks data);

        /// <summary>
        /// Marks a task as completed by its ID
        /// </summary>
        /// <param name="taskId">The ID of the task to mark as done</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure
        /// </returns>
        Task<Result> MarkasDone(int taskId);

        /// <summary>
        /// Retrieves the count of tasks for the specified user or all users if admin
        /// </summary>
        /// <param name="userName">The username of the person whose task count to retrieve</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin</param>
        /// <returns>
        /// List of task counts as <see cref="TaskCount"/> objects
        /// </returns>
        List<TaskCount> TasksCount(string userName, bool isAdmin);
    }
}
