namespace Capstone.Api.Models.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int PackageId { get; set; }
        public DateTime BookingDate { get; set; }
        public required string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public UserDTO? User { get; set; }
        public PackageDTO? Package { get; set; }
        public InquiryDTO? Inquiry { get; set; }
    }
} 