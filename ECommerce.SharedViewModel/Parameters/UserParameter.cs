namespace Ecommerce.SharedViewModel.ParametersType
{
    public class ChangePasswordParameter
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmPassword { get; set; }
    }

    public class UpdateCustomerParameter
    {
        public required int Id {get; set; }
        public required string Name {get; set; }
        public required string Email {get; set; }
        public required string PhoneNumber {get; set; }
        public required string Address {get; set; }
        public required string Username {get; set; }
    }
}