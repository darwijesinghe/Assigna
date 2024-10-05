namespace UserInterface.Models
{
    /// <summary>
    /// View model for password reset
    /// </summary>
    public class ResetViewModel
    {
        
        public class SendResetLink
        {
            /// <summary>
            /// User email
            /// </summary>
            public string Mail { get; set; }
        }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Confirm password
        /// </summary>
        public string Confirm  { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Mail     { get; set; }

        /// <summary>
        /// Reset token
        /// </summary>
        public string Token    { get; set; }
    }
}