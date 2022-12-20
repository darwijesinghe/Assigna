using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataLibrary.Models
{
    // data table
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tsk_id { get; set; }
        [MaxLength(50)]
        public string tsk_title { get; set; } = null!;
        public DateTime deadline { get; set; }
        [MaxLength(250)]
        public string tsk_note { get; set; } = null!;
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
        public Category category { get; set; } = null!;

        public int user_id { get; set; }
        [JsonIgnore]
        [ForeignKey("user_id")]
        public Users users { get; set; } = null!;
    }

    // data transferring model
    public class TaskDto
    {
        public int tsk_id { get; set; }
        public string? tsk_title { get; set; }
        public DateTime deadline { get; set; }
        public string? tsk_note { get; set; }
        public bool pending { get; set; }
        public bool complete { get; set; }
        public bool prio_high { get; set; }
        public bool prio_medium { get; set; }
        public bool prio_low { get; set; }
        public string? user_note { get; set; }
        public string? user_name { get; set; }
        public string? first_name { get; set; }
        public string? user_mail { get; set; }
        public string? cat_name { get; set; }
        public int cat_id { get; set; }
        public int user_id { get; set; }

    }
}