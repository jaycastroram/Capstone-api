using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Api.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        public required string UserId { get; set; }
        public int PackageId { get; set; }
        public int? PhotographerId { get; set; }
        public int? InquiryId { get; set; }

        private DateTime _bookingDate;
        
        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime BookingDate 
        { 
            get => _bookingDate;
            set => _bookingDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        // Helper method to get local time
        [NotMapped]
        public DateTime LocalBookingDate => BookingDate.ToLocalTime();

        [Required]
        public TimeSpan Time { get; set; }

        public string Status { get; set; } = "pending"; // pending, confirmed, canceled
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Photographer? Photographer { get; set; }
        public virtual Package Package { get; set; }
        public virtual Inquiry? Inquiry { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
} 