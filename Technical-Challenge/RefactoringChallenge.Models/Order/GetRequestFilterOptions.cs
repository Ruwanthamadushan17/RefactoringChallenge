using System;
using System.Collections.Generic;
using System.Text;

namespace RefactoringChallenge.Models.Order
{
    public class GetRequestFilterOptions
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
