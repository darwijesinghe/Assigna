using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataLibrary.Models
{
    // data table
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cat_id { get; set; }
        [MaxLength(50)]
        public string cat_name { get; set; } = string.Empty;
        public DateTime insertdate { get; set; }

        // relationships
        [JsonIgnore]
        public List<Task>? task { get; set; }
    }

    // data transferring model
    public class CategoryDto
    {
        public int cat_id { get; set; }
        public string? cat_name { get; set; }
    }
}
