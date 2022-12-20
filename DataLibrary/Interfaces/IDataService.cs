using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary.Models;
using DataLibrary.Response;

namespace DataLibrary.Interfaces
{
    public interface IDataService
    {
        // all tasks
        List<TaskDto> AllTasks(string userName, bool isAdmin);

        // pending taks
        List<TaskDto> Pendings(string userName, bool isAdmin);

        // completed tasks
        List<TaskDto> Completed(string userName, bool isAdmin);

        // high priority tasks
        List<TaskDto> HighPriority(string userName, bool isAdmin);

        // medium priority tasks
        List<TaskDto> MediumPriority(string userName, bool isAdmin);

        // low priority tasks
        List<TaskDto> LowPriority(string userName, bool isAdmin);

        // categories
        List<CategoryDto> Categories();

        // users
        List<UsersDto> Users();

        // save new user
        Task<Result> SaveNewUserAsync(UsersDto data);

        // add a new task
        Task<Result> SaveTaskAsync(TaskDto data);

        // task infomation
        List<TaskDto> TaskInfo(int taskId);

        // edit a task
        Task<Result> EditTaskAsync(TaskDto data);

        // delete a task
        Task<Result> DeleteTaskAsync(int taskId);

        // add task note
        Task<Result> AddTaskNoteAsync(TaskDto data);

        // mark as done
        Task<Result> MarkasDone(int taskId);

        // task count
        List<TaskCount> TaskCount(string userName, bool isAdmin);

    }
}
