namespace Domain.Classes
{
    /// <summary>
    /// Task count domain class.
    /// </summary>
    public class TaskCount
    {
        /// <summary>
        /// Holds all task
        /// </summary>
        public int AllTask  { get; set; }

        /// <summary>
        /// Holds pending task count
        /// </summary>
        public int Pendings { get; set; }

        /// <summary>
        /// Holds completed task count
        /// </summary>
        public int Complete { get; set; }

        /// <summary>
        /// Holds high priority task count
        /// </summary>
        public int High     { get; set; }

        /// <summary>
        /// Holds medium priority task count
        /// </summary>
        public int Medium   { get; set; }

        /// <summary>
        /// Holds low priority task count
        /// </summary>
        public int Low      { get; set; }
    }
}