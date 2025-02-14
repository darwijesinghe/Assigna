using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Classes
{
    /// <summary>
    /// Domain class of user information.
    /// </summary>
    public class Users
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId                 { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [MaxLength(50)]
        public string UserName            { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        [MaxLength(50)]
        public string FirstName           { get; set; }

        /// <summary>
        /// User email address
        /// </summary>
        [MaxLength(50)]
        public string UserMail            { get; set; }

        /// <summary>
        /// Indicates whether the user is an admin or not
        /// </summary>
        public bool IsAdmin               { get; set; }

        /// <summary>
        /// Inserted date
        /// </summary>
        public DateTime InsertDate        { get; set; }

        // Relationship -------------------------------
        [JsonIgnore]
        public ICollection<Tasks> Task    { get; set; }
    }

    /// <summary>
    /// Data transfer model
    /// </summary>
    public class UsersDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        public int UserId       { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName  { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User email address
        /// </summary>
        public string UserMail  { get; set; }

        /// <summary>
        /// Indicates whether the user is an admin or not
        /// </summary>
        public bool IsAdmin     { get; set; }
    }
}