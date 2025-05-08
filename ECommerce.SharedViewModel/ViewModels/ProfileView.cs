using Ecommerce.SharedViewModel.Responses;
using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.SharedViewModel.ViewModels
{
    public class ProfileViewModel
    {
        public CustomerResponse Customer { get; set; }
        public IList<Review> Reviews { get; set; }
        public IList<Order> Orders { get; set; }
    }
}