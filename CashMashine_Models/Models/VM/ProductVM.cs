using CashMashine_Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashMashine_Models.VM
{
    public class ProductVM
    {
        public IEnumerable<Product> Product { get; set; }
        public int Total { get; set; }


    }
}
