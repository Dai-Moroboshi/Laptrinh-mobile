using System.ComponentModel.DataAnnotations;

namespace web_api_p5.DTOs
{
    public class MenuItemCreateRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int PreparationTime { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsSpicy { get; set; }
    }
    
    public class MenuItemUpdateRequest : MenuItemCreateRequest 
    {
        public bool IsAvailable { get; set; }
    }
}
