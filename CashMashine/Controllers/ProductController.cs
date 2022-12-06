using CashMashine_Models;
using CashMashine_Models.VM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CashMashine.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Cart> cartList = _db.Cart;
            int sum = 0;
            foreach (var cart in cartList)
            {
                sum += cart.Total * cart.Count;
            }

            ProductVM prod=new ProductVM();
            prod.Product= _db.Product;
            prod.Total = sum;
            return View(prod);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product prod)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (files.Count != 0)
                {
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    prod.Image = fileName + extension;
                }
                _db.Product.Add(prod);
                _db.SaveChanges();
            }
            return Redirect("~/Maschine/Index/");
        }

        public IActionResult Detail(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var prod=_db.Product.Find(id);
            _db.Product.Remove(prod);
            _db.SaveChanges();
            return Redirect("~/Product/Index/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detail(Product prod)
        {
            if (ModelState.IsValid)
            {
                var obj = _db.Product.AsNoTracking().Where(u => u.Id == prod.Id);
                prod.Image= obj.First().Image;
                _db.Product.Update(prod);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prod);
        }
    }
}
