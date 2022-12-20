using System.Collections.Generic;
using DataLibrary.Models;

namespace UserInterface.Models
{
    public class TaskViewModel
    {
        // data capture list
        public List<TaskDto>? _tasks { get; set; }
        public List<TaskDto>? _pending { get; set; }
        public List<TaskDto>? _complete { get; set; }
        public List<TaskDto>? _hpriority { get; set; }
        public List<TaskDto>? _mpriority { get; set; }
        public List<TaskDto>? _lpriority { get; set; }
        public List<TaskDto>? _taskinfo { get; set; }

        // add new task model
        public class NewTaskViewModel
        {
            public int tsk_id { get; set; }
            public string tsk_title { get; set; } = string.Empty;
            public int tsk_category { get; set; }
            public string deadline { get; set; } = string.Empty;
            public string priority { get; set; } = string.Empty;
            public int assign_to { get; set; }
            public string tsk_note { get; set; } = string.Empty;

            public List<CategoryDto>? _category { get; set; }
            public List<UsersDto>? _users { get; set; }

        }

        // add note to task model
        public class AddNoteViewModel
        {
            public int tsk_id { get; set; }
            public string? user_note { get; set; }
            public List<TaskDto>? _taskinfo { get; set; }
        }

        // send task remind model
        public class RemindViewModel
        {
            public int task_id { get; set; }
            public string? message { get; set; }
        }
    }
}