using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataLibrary.Models
{
    // data table
    public class Task
    {
#pragma warning disable IDE1006 // Naming Styles
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tsk_id { get; set; }
        [MaxLength(50)]
        public string tsk_title { get; set; }
        public DateTime deadline { get; set; }
        [MaxLength(250)]
        public string tsk_note { get; set; }
        public bool pending { get; set; }
        public bool complete { get; set; }
        public bool prio_high { get; set; }
        public bool prio_medium { get; set; }
        public bool prio_low { get; set; }
        [MaxLength(250)]
        public string user_note { get; set; } = string.Empty;
        public DateTime insertdate { get; set; }

        // relationships
        public int cat_id { get; set; }
        [JsonIgnore]
        [ForeignKey("cat_id")]
        public Category category { get; set; }

        public int user_id { get; set; }
        [JsonIgnore]
        [ForeignKey("user_id")]
        public Users users { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    // data transferring model
    public class TaskDto
    {
        public int TskId { get; set; }
        public string? TskTitle { get; set; }
        public DateTime Deadline { get; set; }
        public string? TskNote { get; set; }
        public bool Pending { get; set; }
        public bool Complete { get; set; }
        public bool PrioHigh { get; set; }
        public bool PrioMedium { get; set; }
        public bool PrioLow { get; set; }
        public string? UserNote { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? UserMail { get; set; }
        public string? CatName { get; set; }
        public int CatId { get; set; }
        public int UserId { get; set; }

    }
}