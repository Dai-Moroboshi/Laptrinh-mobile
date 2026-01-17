using System.ComponentModel.DataAnnotations;

namespace web_api_p5.DTOs
{
    public class ReservationCreateRequest
    {
        [Required]
        public DateTime ReservationDate { get; set; }
        [Required]
        [Range(1, 100)]
        public int NumberOfGuests { get; set; }
        public string? SpecialRequests { get; set; }
    }

    public class ReservationItemAddRequest
    {
        [Required]
        public int MenuItemId { get; set; }
        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }

    public class ReservationConfirmRequest
    {
        [Required]
        public string TableNumber { get; set; }
    }

    public class PayReservationRequest
    {
        [Required]
        public string PaymentMethod { get; set; }
        public bool UseLoyaltyPoints { get; set; }
        public int LoyaltyPointsToUse { get; set; }
    }
}
