namespace UserInterface.Models
{
    public class ResetViewModel
    {
        // data capture variables
        public class SendResetLink
        {
            public string Mail { get; set; }
        }
        public string Password { get; set; }
        public string Confirm { get; set; }
        public string Mail { get; set; }
        public string Token { get; set; }
    }
}