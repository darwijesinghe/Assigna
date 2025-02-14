using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Classes
{
    /// <summary>
    /// Domain class for categories.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CatId            { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        [MaxLength(50)]
        public string CatName       { get; set; } = string.Empty;

        /// <summary>
        /// Inserted date
        /// </summary>
        public DateTime InsertDate  { get; set; }

        // Relationship -------------------------

        [JsonIgnore]
        public List<Tasks>? Task     { get; set; }
    }

    /// <summary>
    /// Data transfer model
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Category ID
        /// </summary>
        public int CatId        { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        public string? CatName  { get; set; }
    }
}
