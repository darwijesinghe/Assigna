namespace UserInterface.Models
{
    public class ResetViewModel
    {
        // data capture variables
        public class SendResetLink
        {
            public string mail { get; set; } = string.Empty;
        }
        public string password { get; set; } = string.Empty;
        public string confirm { get; set; } = string.Empty;
        public string mail { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
    }
}