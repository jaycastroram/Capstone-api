using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public int PhotographerId { get; set; }
        public int PackageId { get; set; }
        public int? InquiryId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        public string Status { get; set; } = "pending"; // pending, confirmed, canceled

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Photographer Photographer { get; set; }
        public virtual Package Package { get; set; }
        public virtual Inquiry? Inquiry { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
} 