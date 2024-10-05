﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Classes
{
    /// <summary>
    /// Domain class for tasks
    /// </summary>
    public class Tasks
    {
        /// <summary>
        /// Priamry key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId           { get; set; }

        /// <summary>
        /// Task title
        /// </summary>
        [MaxLength(50)]
        public string TaskTitle     { get; set; }

        /// <summary>
        /// Task deadline
        /// </summary>
        public DateTime Deadline    { get; set; }

        /// <summary>
        /// Task note
        /// </summary>
        [MaxLength(250)]
        public string TaskNote      { get; set; }

        /// <summary>
        /// Pending task flag
        /// </summary>
        public bool Pending         { get; set; }

        /// <summary>
        /// Completed task flag
        /// </summary>
        public bool Complete        { get; set; }

        /// <summary>
        /// High priority task
        /// </summary>
        public bool HighPriority    { get; set; }

        /// <summary>
        /// Medium priority task
        /// </summary>
        public bool MediumPriority  { get; set; }

        /// <summary>
        /// Low priority task
        /// </summary>
        public bool LowPriority     { get; set; }

        /// <summary>
        /// User note for the task
        /// </summary>
        [MaxLength(250)]
        public string? UserNote     { get; set; }

        /// <summary>
        /// Task insrted date
        /// </summary>
        public DateTime InsertDate  { get; set; }

        // Relationship -------------------------
        public int CatId            { get; set; }

        [JsonIgnore]
        [ForeignKey("CatId")]
        public Category Category    { get; set; }

        public int UserId           { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public Users Users          { get; set; }
    }

    /// <summary>
    /// Data transfer model
    /// </summary>
    public class TasksDto
    {
        /// <summary>
        /// Task ID
        /// </summary>
        public int TaskId          { get; set; }

        /// <summary>
        /// Task title
        /// </summary>
        public string TaskTitle    { get; set; }

        /// <summary>
        /// Task deadline
        /// </summary>
        public DateTime Deadline   { get; set; }

        /// <summary>
        /// Task note
        /// </summary>
        public string TaskNote     { get; set; }

        /// <summary>
        /// Pending task
        /// </summary>
        public bool Pending        { get; set; }

        /// <summary>
        /// Completed task
        /// </summary>
        public bool Complete       { get; set; }

        /// <summary>
        /// High priority task
        /// </summary>
        public bool HighPriority   { get; set; }

        /// <summary>
        /// Medium priority task
        /// </summary>
        public bool MediumPriority { get; set; }

        /// <summary>
        /// Low priority task
        /// </summary>
        public bool LowPriority    { get; set; }

        /// <summary>
        /// User note for the task
        /// </summary>
        public string? UserNote    { get; set; }

        /// <summary>
        /// Task asignee name
        /// </summary>
        public string UserName     { get; set; }

        /// <summary>
        /// Task asignee first name
        /// </summary>
        public string FirstName    { get; set; }

        /// <summary>
        /// Task asignee mail
        /// </summary>
        public string UserMail     { get; set; }

        /// <summary>
        /// Task category name
        /// </summary>
        public string CatName      { get; set; }

        /// <summary>
        /// Category ID
        /// </summary>
        public int CatId           { get; set; }

        /// <summary>
        /// Task asignee ID
        /// </summary>
        public int UserId          { get; set; }
    }
}