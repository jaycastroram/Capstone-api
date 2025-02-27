using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public int BookingId { get; set; }
        public string UserId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Status { get; set; } = "pending"; // pending, paid, refunded

        public string? TransactionId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Booking Booking { get; set; }
        public virtual User User { get; set; }
    }
} 