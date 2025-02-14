namespace UserInterface.Models
{
    /// <summary>
    /// View model for user signup.
    /// </summary>
    public class SignUpViewModel
    {
        /// <summary>
        /// User first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName  { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password  { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email     { get; set; }

        /// <summary>
        /// User role
        /// </summary>
        public string Role      { get; set; }
    }
}