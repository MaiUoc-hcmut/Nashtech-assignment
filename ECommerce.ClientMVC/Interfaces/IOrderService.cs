using Ecommerce.SharedViewModel.Responses;
using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.ClientMVC.Interface
{
    public interface IOrderService
    {
        Task<CreateOrderResponse?> CreateOrder
        (
            string customerName,
            string customerEmail,
            string customerPhoneNumber,
            string customerAddress,
            string variantIdList,
            int totalAmount 
        );
        Task<IEnumerable<GetOrdersOfCustomerResponse>> GetOrdersOfCustomer(int customerId);
        // Task<bool> CancelOrder(int orderId);
        // Task<bool> UpdateOrderStatus(int orderId, string status);
        // Task<bool> GetOrderDetails(int orderId);
    }
}