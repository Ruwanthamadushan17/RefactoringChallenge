using RefactoringChallenge.Models.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefactoringChallenge.Services.Order
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> Get(GetRequestFilterOptions getRequestFilterOptions);

        Task<OrderResponse> GetById(int orderId);

        Task<IEnumerable<OrderDetailResponse>> AddProductsToOrder(int orderId, IEnumerable<OrderDetailRequest> orderDetails);

        Task Delete(int orderId);

        Task<OrderResponse> Create(CreateOrderRequest createOrderRequest);
    }
}
