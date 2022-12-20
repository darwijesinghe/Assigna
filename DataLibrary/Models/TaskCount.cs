namespace DataLibrary.Models
{
    // all type of task count
    public class TaskCount
    {
        public int all_task { get; set; }
        public int pendings { get; set; }
        public int complete { get; set; }
        public int high { get; set; }
        public int medium { get; set; }
        public int low { get; set; }
    }
}