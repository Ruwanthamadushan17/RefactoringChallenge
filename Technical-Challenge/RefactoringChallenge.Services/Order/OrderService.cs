using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Entities;
using RefactoringChallenge.Entities.Entities;
using RefactoringChallenge.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ent = RefactoringChallenge.Entities.Entities;

namespace RefactoringChallenge.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly NorthwindDbContext _northwindDbContext;
        private readonly IMapper _mapper;

        public OrderService(NorthwindDbContext northwindDbContext, IMapper mapper)
        {
            _northwindDbContext = northwindDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponse>> Get(GetRequestFilterOptions getRequestFilterOptions)
        {
            IQueryable<ent.Order> query = _northwindDbContext.Orders;
            if (getRequestFilterOptions.Skip != null)
            {
                query.Skip(getRequestFilterOptions.Skip.Value);
            }
            if (getRequestFilterOptions.Take != null)
            {
                query.Take(getRequestFilterOptions.Take.Value);
            }

            return await _mapper.From(query).ProjectToType<OrderResponse>().ToListAsync();
        }

        public async Task<OrderResponse> GetById(int orderId)
        {
            return _mapper.Map<OrderResponse>(await _northwindDbContext.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderId == orderId));
        }

        public async Task<OrderResponse> Create(CreateOrderRequest createOrderRequest)
        {
            var newOrderDetails = new List<OrderDetail>();
            foreach (var orderDetail in createOrderRequest.OrderDetails)
            {
                newOrderDetails.Add(_mapper.Map<OrderDetail>(orderDetail));
            }

            var newOrder = _mapper.Map<ent.Order>(createOrderRequest);
            newOrder.OrderDetails = newOrderDetails;

            await _northwindDbContext.Orders.AddAsync(newOrder);
            await _northwindDbContext.SaveChangesAsync();

            return newOrder.Adapt<OrderResponse>();
        }

        public async Task<IEnumerable<OrderDetailResponse>> AddProductsToOrder(int orderId, IEnumerable<OrderDetailRequest> orderDetails)
        {
            var order = await _northwindDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
                throw new System.Exception("No order found.");

            var newOrderDetails = new List<OrderDetail>();
            foreach (var orderDetail in orderDetails)
            {
                var orderDetailEnt = _mapper.Map<OrderDetail>(orderDetail);
                orderDetailEnt.OrderId = orderId;
                newOrderDetails.Add(orderDetailEnt);
            }

            await _northwindDbContext.OrderDetails.AddRangeAsync(newOrderDetails);
            await _northwindDbContext.SaveChangesAsync();

            return newOrderDetails.Select(od => od.Adapt<OrderDetailResponse>());
        }

        public async Task Delete(int orderId)
        {
            var order = await _northwindDbContext.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
                throw new System.Exception("No order found.");

            _northwindDbContext.OrderDetails.RemoveRange(order.OrderDetails);
            _northwindDbContext.Orders.Remove(order);
            await _northwindDbContext.SaveChangesAsync();
        }
    }
}
