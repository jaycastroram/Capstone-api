using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class PhotographerStyle
    {
        public int PhotographerId { get; set; }
        public int StyleId { get; set; }

        // Navigation properties
        public virtual Photographer Photographer { get; set; }
        public virtual Style Style { get; set; }
    }
} 