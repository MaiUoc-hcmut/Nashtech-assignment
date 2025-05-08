namespace Ecommerce.SharedViewModel.ParametersType
{
    public class RegisterParameter
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string Username { get; set; } = string.Empty;
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class LoginParameter
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class RefreshTokenParameter
    {
        
    }
}