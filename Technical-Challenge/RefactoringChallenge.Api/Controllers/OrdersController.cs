using Microsoft.AspNetCore.Mvc;
using RefactoringChallenge.Models.Order;
using RefactoringChallenge.Services.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefactoringChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Get([FromBody] GetRequestFilterOptions getRequestFilterOptions)
        {
            return new OkObjectResult(await _orderService.Get(getRequestFilterOptions));
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetById([FromRoute] int orderId)
        {
            var result = await _orderService.GetById(orderId);

            if (result == null)
                return NotFound();

            return new OkObjectResult(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest createOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return new OkObjectResult(await _orderService.Create(createOrderRequest));
        }

        [HttpPatch("{orderId}/[action]")]
        public async Task<IActionResult> AddProductsToOrder([FromRoute] int orderId, IEnumerable<OrderDetailRequest> orderDetails)
        {
            return new OkObjectResult(await _orderService.AddProductsToOrder(orderId, orderDetails));
        }

        [HttpDelete("{orderId}/[action]")]
        public async Task<IActionResult> Delete([FromRoute] int orderId)
        {
            await _orderService.Delete(orderId);

            return new OkResult();
        }
    }
}
