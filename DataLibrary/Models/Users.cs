using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataLibrary.Models
{
    // data table -> extra table to save user info
    public class Users
    {
#pragma warning disable IDE1006 // Naming Styles
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }
        [MaxLength(50)]
        public string user_name { get; set; }
        [MaxLength(50)]
        public string first_name { get; set; }
        [MaxLength(50)]
        public string user_mail { get; set; }
        public bool is_admin { get; set; }
        public DateTime insertdate { get; set; }

        // relationships
        [JsonIgnore]
        public List<Task>? task { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    // data transferring model
    public class UsersDto
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? UserMail { get; set; }
        public bool IsAdmin { get; set; }
    }
}