using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_api_p5.Models
{
    [Table("reservations")]
    public class Reservation
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required]
        [Column("reservation_number")]
        public string ReservationNumber { get; set; }

        [Required]
        [Column("reservation_date")]
        public DateTime ReservationDate { get; set; }

        [Required]
        [Column("number_of_guests")]
        public int NumberOfGuests { get; set; }

        [Column("table_number")]
        public string TableNumber { get; set; }

        [Column("status")]
        public string Status { get; set; } = "pending"; // pending, confirmed, seated, completed, cancelled, no_show

        [Column("special_requests")]
        public string SpecialRequests { get; set; }

        [Column("subtotal", TypeName = "decimal(18, 2)")]
        public decimal Subtotal { get; set; } = 0;

        [Column("service_charge", TypeName = "decimal(18, 2)")]
        public decimal ServiceCharge { get; set; } = 0;

        [Column("discount", TypeName = "decimal(18, 2)")]
        public decimal Discount { get; set; } = 0;

        [Column("total", TypeName = "decimal(18, 2)")]
        public decimal Total { get; set; } = 0;

        [Column("payment_method")]
        public string PaymentMethod { get; set; }

        [Column("payment_status")]
        public string PaymentStatus { get; set; } = "pending"; // pending, paid, refunded

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ReservationItem> ReservationItems { get; set; }
    }
}
