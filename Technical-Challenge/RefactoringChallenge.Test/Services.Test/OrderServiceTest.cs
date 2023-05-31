using AutoFixture;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Entities;
using RefactoringChallenge.Entities.Entities;
using RefactoringChallenge.Models.Order;
using RefactoringChallenge.Services.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RefactoringChallenge.Test.Services.Test
{
    public class OrderServiceTest : IClassFixture<DatabaseFixture>
    {
        private readonly IMapper _mapper;
        DatabaseFixture _databaseFixture;
        private readonly Fixture _fixture;

        public OrderServiceTest(DatabaseFixture databaseFixture)
        {
            _mapper = new Mapper();
            _databaseFixture = databaseFixture;
            _fixture = new Fixture();
        }


        [Fact]
        public void Get_Returns_AllOrders_When_FilterOptionsIsNull()
        {
            OrderService orderService = new OrderService(_databaseFixture._dbContext, _mapper);
            var response = orderService.Get(new GetRequestFilterOptions());

            Assert.True(response.Result.Count() > 0);
        }

        [Fact]
        public void Get_Returns_AllOrders_When_FilterOptionsIsNotNull()
        {
            OrderService orderService = new OrderService(_databaseFixture._dbContext, _mapper);
            var response = orderService.Get(new GetRequestFilterOptions() { Skip = 0, Take = 5});

            Assert.True(response.Result.Count() > 0);
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public void GetById_Returns_MatchingOrder(int orderId)
        {
            OrderService orderService = new OrderService(_databaseFixture._dbContext, _mapper);
            var response = orderService.GetById(orderId);

            Assert.NotNull(response);
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void Create_Returns_OrderResponse_OnSuccess()
        {
            var fakeOrderDatailRequest = _fixture.Build<OrderDetailRequest>()
                                                    .With(o => o.ProductId, 4)
                                                    .Create();

            var fakeOrderCreateRequest = _fixture.Build<CreateOrderRequest>()
                                                    .With(o => o.CustomerId, "4")
                                                    .With(o => o.EmployeeId, 4)
                                                    .With(o => o.OrderDetails, new List<OrderDetailRequest>() { fakeOrderDatailRequest })
                                                    .Create(); 

            OrderService orderService = new OrderService(_databaseFixture._dbContext, _mapper);
            var response = orderService.Create(fakeOrderCreateRequest);

            Assert.True(response.Result.CustomerId == "4");
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void AddProductsToOrder_Returns_OrderDetailResponse_OnSuccess()
        {
            var fakeOrderDatailRequest = _fixture.Build<OrderDetailRequest>()
                                                    .With(o => o.ProductId, 1)
                                                    .Create();

            var orderId = 1;

            OrderService orderService = new OrderService(_databaseFixture._dbContext, _mapper);
            var response = orderService.AddProductsToOrder(orderId, new List<OrderDetailRequest>() { fakeOrderDatailRequest });

            Assert.True(response.Result.Count() == 1);
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void AddProductsToOrder_Returns_404_OnOrderNotFound()
        {
            var fakeOrderDatailRequest = _fixture.Build<OrderDetailRequest>()
                                                    .Create();

            var orderId = 10;

            OrderService orderService = new OrderService(_databaseFixture._dbContext, _mapper);
            var response = orderService.AddProductsToOrder(orderId, new List<OrderDetailRequest>() { fakeOrderDatailRequest });

            Assert.True(response.IsFaulted);
        }

        [Fact]
        public void Delete_Should_Success()
        {
            var orderId = 1;

            OrderService orderService = new OrderService(_databaseFixture._dbContext, _mapper);
            var response = orderService.Delete(orderId);

            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void Delete_Should_Throw()
        {
            var orderId = 10;

            OrderService orderService = new OrderService(_databaseFixture._dbContext, _mapper);
            var response = orderService.Delete(orderId);

            Assert.True(response.IsFaulted);
        }
    }

    public class DatabaseFixture : IDisposable
    {
        private static DbContextOptions<NorthwindDbContext> _dbContextOptions;
        public readonly NorthwindDbContext _dbContext;
        public DatabaseFixture()
        {
            _dbContextOptions = new DbContextOptionsBuilder<NorthwindDbContext>().UseInMemoryDatabase(databaseName: "RefactoringChallengeDB").Options;
            _dbContext = new NorthwindDbContext(_dbContextOptions);
            SetupTheDatabase();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        private void SetupTheDatabase()
        {
            _dbContext.Orders.AddRange(GetFakeData().AsQueryable());
            _dbContext.SaveChanges();
        }

        private List<Order> GetFakeData()
        {

            var orders = new List<Order> {
                new Order { CustomerId = "1", EmployeeId = 1, OrderId=1 },
                new Order { CustomerId = "2", EmployeeId = 2, OrderId=2 },
                new Order { CustomerId = "3", EmployeeId = 3, OrderId=3 }
            };
            return orders;
        }
    }

}
