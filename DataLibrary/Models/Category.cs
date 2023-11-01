﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataLibrary.Models
{
    // data table
    public class Category
    {
#pragma warning disable IDE1006 // Naming Styles
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cat_id { get; set; }
        [MaxLength(50)]
        public string cat_name { get; set; } = string.Empty;
        public DateTime insertdate { get; set; }

        // relationships
        [JsonIgnore]
        public List<Task>? task { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    // data transferring model
    public class CategoryDto
    {
        public int CatId { get; set; }
        public string? CatName { get; set; }
    }
}
