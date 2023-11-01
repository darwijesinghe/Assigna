namespace DataLibrary.Models
{
    // all type of task count
    public class TaskCount
    {
        public int AllTask { get; set; }
        public int Pendings { get; set; }
        public int Complete { get; set; }
        public int High { get; set; }
        public int Medium { get; set; }
        public int Low { get; set; }
    }
}