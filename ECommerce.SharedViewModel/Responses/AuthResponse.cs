using Ecommerce.SharedViewModel.DTOs;


namespace Ecommerce.SharedViewModel.Responses
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public CustomerDTO Customer { get; set; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public CustomerDTO Customer { get; set; }
    }
}