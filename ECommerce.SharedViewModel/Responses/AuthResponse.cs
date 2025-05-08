using Ecommerce.SharedViewModel.DTOs;


namespace Ecommerce.SharedViewModel.Responses
{
    public class CustomerResponse
    {
        public int CartId { get; set; }
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
    }

    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public CustomerResponse? Customer { get; set; }
        public AdminDTO? Admin { get; set; }
    }
}