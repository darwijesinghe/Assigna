using System.Collections.Generic;
using Domain.Classes;

namespace UserInterface.Models
{
    /// <summary>
    /// View model for layout
    /// </summary>
    public class LayoutViewModel
    {
        /// <summary>
        /// User name
        /// </summary>
        public string UserName           { get; set; }

        /// <summary>
        /// User role
        /// </summary>
        public bool Role                 { get; set; }

        /// <summary>
        /// Task count
        /// </summary>
        public List<TaskCount> TaskCount { get; set; }
    }
}