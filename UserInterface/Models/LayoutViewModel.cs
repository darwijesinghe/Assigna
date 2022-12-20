using System.Collections.Generic;
using DataLibrary.Models;

namespace UserInterface.Models
{
    public class LayoutViewModel
    {
        // data capture variables
        public string? user_name { get; set; }
        public bool role { get; set; }
        public List<TaskCount>? _taskcount { get; set; }
    }
}