using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_api_p5.Models
{
    [Table("menu_items")]
    public class MenuItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Required]
        [Column("category")]
        public string Category { get; set; } // "Appetizer", "Main Course", "Dessert", "Beverage", "Soup"

        [Required]
        [Column("price", TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Column("preparation_time")]
        public int PreparationTime { get; set; }

        [Column("is_vegetarian")]
        public bool IsVegetarian { get; set; } = false;

        [Column("is_spicy")]
        public bool IsSpicy { get; set; } = false;

        [Column("is_available")]
        public bool IsAvailable { get; set; } = true;

        [Column("rating", TypeName = "decimal(3, 1)")]
        public decimal Rating { get; set; } = 0.0m;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
