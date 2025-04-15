namespace Ecommerce.SharedViewModel.ParametersType
{
    public class RegisterParameter
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class LoginParameter
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RefreshTokenParameter
    {
        
    }
}