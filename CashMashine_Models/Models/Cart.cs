using CashMashine_Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CashMashine_Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
    }
}
