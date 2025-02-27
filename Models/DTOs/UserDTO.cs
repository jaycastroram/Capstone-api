namespace Capstone.Api.Models.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public bool IsVerified { get; set; }
        public string? ImageLocation { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
} 