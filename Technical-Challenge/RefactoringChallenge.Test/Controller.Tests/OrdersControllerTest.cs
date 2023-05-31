using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RefactoringChallenge.Controllers;
using RefactoringChallenge.Models.Order;
using RefactoringChallenge.Services.Order;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RefactoringChallenge.Test.Controller.Tests
{
    public class OrdersControllerTest
    {
        private readonly Mock<IOrderService> _orderService;
        private readonly Fixture _fixture;
        public OrdersControllerTest()
        {
            _orderService = new Mock<IOrderService>();
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Get_Returns_Orders()
        {
            var GetRequest = _fixture.Build<GetRequestFilterOptions>()
                                        .With(o => o.Skip, 0)
                                        .With(o => o.Take, 5)
                                        .Create();

            var fakeOrderResponse = _fixture.Create<OrderResponse>();

            _orderService.Setup(o => o.Get(It.IsAny<GetRequestFilterOptions>()))
                            .ReturnsAsync(new List<OrderResponse>() { fakeOrderResponse });

            OrdersController ordersController = new OrdersController(_orderService.Object);
            var response = await ordersController.Get(GetRequest);

            Assert.NotNull(response);
            _orderService.Verify(
                            x => x.Get(It.IsAny<GetRequestFilterOptions>()),
                            Times.Once);
        }

        [Fact]
        public async Task GetById_Returns_MatchingOrder()
        {
            var orderId = _fixture.Create<int>();
            var fakeOrderResponse = _fixture.Create<OrderResponse>();

            _orderService.Setup(o => o.GetById(It.IsAny<int>()))
                            .ReturnsAsync(fakeOrderResponse);

            OrdersController ordersController = new OrdersController(_orderService.Object);
            var result = await ordersController.GetById(orderId);

            Assert.NotNull(result);
            _orderService.Verify(
                            x => x.GetById(It.IsAny<int>()),
                            Times.Once);
        }

        [Fact]
        public async Task GetById_Returns_404_ForNonMatchingOrder()
        {
            var orderId = _fixture.Create<int>();

            _orderService.Setup(o => o.GetById(It.IsAny<int>()))
                            .ReturnsAsync(null as OrderResponse);

            OrdersController ordersController = new OrdersController(_orderService.Object);
            var result = await ordersController.GetById(orderId);

            Assert.NotNull(result);
            _orderService.Verify(
                            x => x.GetById(It.IsAny<int>()),
                            Times.Once);
        }

        [Fact]
        public async Task Create_ShouldSuccess()
        {
            var orderResponse = _fixture.Create<OrderResponse>();
            var orderCreateRequest = _fixture.Create<CreateOrderRequest>();

            _orderService.Setup(o => o.Create(It.IsAny<CreateOrderRequest>()))
                            .ReturnsAsync(orderResponse);

            OrdersController ordersController = new OrdersController(_orderService.Object);
            var result = await ordersController.Create(orderCreateRequest);

            Assert.NotNull(result);
            _orderService.Verify(
                            x => x.Create(It.IsAny<CreateOrderRequest>()),
                            Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturn400()
        {
            var orderCreateRequest = _fixture.Build<CreateOrderRequest>()
                                                .With(o => o.CustomerId, "Invalid Customer Id")
                                                .Create(); 

            OrdersController ordersController = new OrdersController(_orderService.Object);
            ordersController.ModelState.AddModelError("CustomerId", "Only Allowed 5 characters");

            var result = await ordersController.Create(orderCreateRequest);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddProductsToOrder_ShouldSuccess()
        {
            var orderDetailRequest = _fixture.Create<OrderDetailRequest>();
            var orderId = _fixture.Create<int>();
            var orderDetailResponse = _fixture.Create<OrderDetailResponse>();

            _orderService.Setup(o => o.AddProductsToOrder(It.IsAny<int>(), It.IsAny<IEnumerable<OrderDetailRequest>>()))
                            .ReturnsAsync(new List<OrderDetailResponse>() { orderDetailResponse });

            OrdersController ordersController = new OrdersController(_orderService.Object);
            var result = await ordersController.AddProductsToOrder(orderId, new List<OrderDetailRequest>() { orderDetailRequest });

            Assert.NotNull(result);
            _orderService.Verify(
                            x => x.AddProductsToOrder(It.IsAny<int>(), It.IsAny<IEnumerable<OrderDetailRequest>>()),
                            Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldSuccess()
        {
            var orderId = _fixture.Create<int>();

            _orderService.Setup(o => o.Delete(It.IsAny<int>()))
                            .Returns(Task.CompletedTask);

            OrdersController ordersController = new OrdersController(_orderService.Object);
            var result = await ordersController.Delete(orderId);

            Assert.NotNull(result);
            _orderService.Verify(
                            x => x.Delete(It.IsAny<int>()),
                            Times.Once);
        }
    }
}
