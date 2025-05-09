namespace Ecommerce.SharedViewModel.Models
{
    public class Admin : IUserInterface
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public required string Name { get; set; }
        public required string Password { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}