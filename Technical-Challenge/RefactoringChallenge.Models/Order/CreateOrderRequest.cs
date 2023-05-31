using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RefactoringChallenge.Models.Order
{
    public class CreateOrderRequest
    {
        [StringLength(5)]
        public string CustomerId { get; set; }

        public int? EmployeeId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? RequiredDate { get; set; }
        public int? ShipVia { get; set; }

        [DataType(DataType.Currency)]
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }

        [StringLength(60)]
        public string ShipAddress { get; set; }

        [StringLength(15)]
        public string ShipCity { get; set; }

        [StringLength(15)]
        public string ShipRegion { get; set; }

        [StringLength(10)]
        public string ShipPostalCode { get; set; }

        [StringLength(15)]
        public string ShipCountry { get; set; }
        public IEnumerable<OrderDetailRequest> OrderDetails { get; set; }
    }
}
