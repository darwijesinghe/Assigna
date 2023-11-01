using System.Collections.Generic;
using DataLibrary.Models;

namespace UserInterface.Models
{
    public class TaskViewModel
    {
        // data capture list
        public List<TaskDto>? Tasks { get; set; }
        public List<TaskDto>? Pending { get; set; }
        public List<TaskDto>? Complete { get; set; }
        public List<TaskDto>? Hpriority { get; set; }
        public List<TaskDto>? Mpriority { get; set; }
        public List<TaskDto>? Lpriority { get; set; }
        public List<TaskDto>? Taskinfo { get; set; }

        // add new task model
        public class NewTaskViewModel
        {
            public int TskId { get; set; }
            public string TskTitle { get; set; }
            public int TskCategory { get; set; }
            public string Deadline { get; set; }
            public string Priority { get; set; }
            public int AssignTo { get; set; }
            public string TskNote { get; set; }

            public List<CategoryDto>? Category { get; set; }
            public List<UsersDto>? Users { get; set; }

        }

        // add note to task model
        public class AddNoteViewModel
        {
            public int TskId { get; set; }
            public string? UserNote { get; set; }
            public List<TaskDto>? Taskinfo { get; set; }
        }

        // send task remind model
        public class RemindViewModel
        {
            public int TaskId { get; set; }
            public string? Message { get; set; }
        }
    }
}