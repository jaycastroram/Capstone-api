using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Capstone.Api.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public string Role { get; set; } = "user";
        
        public bool IsVerified { get; set; } = false;

        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        [DataType(DataType.Url)]
        [MaxLength(255)]
        public string? ImageLocation { get; set; }
        
        public string FullName => $"{FirstName} {LastName}";
        
        // Navigation properties
        public virtual Photographer? Photographer { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
} 