using System.Collections.Generic;
using Domain.Classes;
using Domain.Enums;

namespace UserInterface.Models
{
    /// <summary>
    /// View model for tasks
    /// </summary>
    public class TaskViewModel
    {
        /// <summary>
        /// Task list
        /// </summary>
        public List<TasksDto> Tasks          { get; set; }

        /// <summary>
        /// Pending task list
        /// </summary>
        public List<TasksDto> Pending        { get; set; }

        /// <summary>
        /// Completed task list
        /// </summary>
        public List<TasksDto> Complete       { get; set; }

        /// <summary>
        /// High priority task list
        /// </summary>
        public List<TasksDto> HighPriority   { get; set; }

        /// <summary>
        /// Medium priority task list
        /// </summary>
        public List<TasksDto> MediumPriority { get; set; }

        /// <summary>
        /// Low priority task list
        /// </summary>
        public List<TasksDto> LowPriority    { get; set; }

        /// <summary>
        /// Task info list
        /// </summary>
        public List<TasksDto> TaskInfo       { get; set; }

        /// <summary>
        /// View model for add new task
        /// </summary>
        public class NewTaskViewModel
        {
            /// <summary>
            /// Task ID
            /// </summary>
            public int TaskId                 { get; set; }

            /// <summary>
            /// Task title
            /// </summary>
            public string TaskTitle           { get; set; }

            /// <summary>
            /// Task category ID
            /// </summary>
            public int CatId                  { get; set; }

            /// <summary>
            /// Deadline of the task
            /// </summary>
            public string Deadline            { get; set; }

            /// <summary>
            /// Task priority
            /// </summary>
            public TaskType Priority          { get; set; }

            /// <summary>
            /// Task assignee
            /// </summary>
            public int AssignTo               { get; set; }

            /// <summary>
            /// Extra note for the task
            /// </summary>
            public string TaskNote            { get; set; }

            /// <summary>
            /// Categories
            /// </summary>
            public List<CategoryDto> Category { get; set; }

            /// <summary>
            /// Team members
            /// </summary>
            public List<UsersDto> Users       { get; set; }

        }

        /// <summary>
        /// View model for add task note
        /// </summary>
        public class AddNoteViewModel
        {
            /// <summary>
            /// Task ID
            /// </summary>
            public int TaskId              { get; set; }

            /// <summary>
            /// User note for the task
            /// </summary>
            public string UserNote         { get; set; }

            /// <summary>
            /// Task info list
            /// </summary>
            public List<TasksDto> Taskinfo { get; set; }
        }

        /// <summary>
        /// View model for send reminders
        /// </summary>
        public class RemindViewModel
        {
            /// <summary>
            /// Task ID
            /// </summary>
            public int TaskId     { get; set; }

            /// <summary>
            /// Reminder message
            /// </summary>
            public string Message { get; set; }
        }
    }
}