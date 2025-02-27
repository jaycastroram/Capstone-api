using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public int PhotographerId { get; set; }

        [Required]
        public string Comment { get; set; }

        public int? Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Photographer Photographer { get; set; }
    }
} 