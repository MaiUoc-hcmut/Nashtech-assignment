using Ecommerce.SharedViewModel.Responses;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;

namespace Ecommerce.ClientMVC.Interface
{
    public interface IOrderService
    {
        Task<CreateOrderResponse?> CreateOrderAsync
        (
            string customerName,
            string customerEmail,
            string customerPhoneNumber,
            string customerAddress,
            IList<VariantInCreateOrder> variantList,
            int totalAmount 
        );
        Task<IEnumerable<GetOrdersOfCustomerResponse>> GetOrdersOfCustomerAsync(int customerId);
        // Task<bool> CancelOrder(int orderId);
        // Task<bool> UpdateOrderStatus(int orderId, string status);
        // Task<bool> GetOrderDetails(int orderId);
    }
}