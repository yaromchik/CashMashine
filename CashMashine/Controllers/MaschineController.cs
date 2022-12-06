using CashMashine_Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CashMashine.Controllers
{
    public class MaschineController : Controller
    {
        private readonly ApplicationDbContext _db;
        public MaschineController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            Program.start = DateTime.Now;
            _db.Cart.RemoveRange(_db.Cart);
            _db.SaveChanges();
            IEnumerable<Cart> cartList = _db.Cart;
            int sum = 0;
            foreach (var cart in cartList)
            {
                sum += cart.Total * cart.Count;
            }
                       
            return View(sum);
        }
    }
}
