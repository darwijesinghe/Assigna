namespace DataLibrary.Response
{
    // response return class
    public class Result
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int Id { get; set; }
    }
    public class Result<T> where T : class
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int Id { get; set; }
        public T? Data { get; set; }
    }
}