namespace UserInterface.Models
{
    public class SignInViewModel
    {
        // data caputure variables
        public string user_name { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string? provider_name { get; set; }
    }
}