namespace Ecommerce.SharedViewModel.ParametersType
{
    public class RegisterParameter
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
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