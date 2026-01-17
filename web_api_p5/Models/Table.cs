using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_api_p5.Models
{
    [Table("tables")]
    public class TableEntity // Renamed to TableEntity to avoid conflict with System.ComponentModel.DataAnnotations.Schema.Table
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("table_number")]
        public string TableNumber { get; set; }

        [Required]
        [Column("capacity")]
        public int Capacity { get; set; }

        [Column("is_available")]
        public bool IsAvailable { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
