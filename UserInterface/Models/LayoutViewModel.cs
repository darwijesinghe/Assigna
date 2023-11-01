using System.Collections.Generic;
using DataLibrary.Models;

namespace UserInterface.Models
{
    public class LayoutViewModel
    {
        // data capture variables
        public string? UserName { get; set; }
        public bool Role { get; set; }
        public List<TaskCount>? TaskCount { get; set; }
    }
}