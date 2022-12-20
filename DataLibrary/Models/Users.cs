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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }
        [MaxLength(50)]
        public string user_name { get; set; } = null!;
        [MaxLength(50)]
        public string first_name { get; set; } = null!;
        [MaxLength(50)]
        public string user_mail { get; set; } = null!;
        public bool is_admin { get; set; }
        public DateTime insertdate { get; set; }

        // relationships
        [JsonIgnore]
        public List<Task>? task { get; set; }
    }

    // data transferring model
    public class UsersDto
    {
        public int user_id { get; set; }
        public string? user_name { get; set; }
        public string? first_name { get; set; }
        public string? user_mail { get; set; }
        public bool is_admin { get; set; }
    }
}