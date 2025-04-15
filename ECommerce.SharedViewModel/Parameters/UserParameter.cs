namespace Ecommerce.SharedViewModel.ParametersType
{
    public class ChangePasswordParameter
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}