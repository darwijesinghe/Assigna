namespace UserInterface.Models
{
    /// <summary>
    /// View model for user signin
    /// </summary>
    public class SignInViewModel
    {
        /// <summary>
        /// User name
        /// </summary>
        public string UserName     { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password     { get; set; }

        /// <summary>
        /// External login provider name
        /// </summary>
        public string ProviderName { get; set; }
    }
}