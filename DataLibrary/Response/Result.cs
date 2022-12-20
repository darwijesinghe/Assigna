namespace DataLibrary.Response
{
    // response return class
    public class Result
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public int id { get; set; }
    }
    public class Result<T> where T : class
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public int id { get; set; }
        public T? Data { get; set; }
    }
}