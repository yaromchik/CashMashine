using CashMashine_Models;
using CashMashine_Models.VM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using System.IO;

namespace CashMashine.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult AddToCart(int id)
        {
            if(id != 0)
            {
                Cart cart = new Cart();
                if (_db.Cart.Where(u => u.IdProduct == id).Count() != 0)
                {
                    Cart updateCart = _db.Cart.Where(u => u.IdProduct == id).First();
                    updateCart.Count++;
                    _db.Cart.Update(updateCart);
                    _db.SaveChanges();
                }
                else
                {
                    cart.Total = _db.Product.Find(id).Cost;
                    cart.Count = 1;
                    cart.IdProduct = id;
                    _db.Cart.Add(cart);
                    _db.SaveChanges();
                }
            }
            return Redirect("~/Product/Index/");
        }
        public IActionResult RemoveAll()
        {
            _db.Cart.RemoveRange(_db.Cart);
            _db.SaveChanges();
            return Redirect("~/Product/Index/");
        }
        public IActionResult Index()
        {
            CartVM cartVM = new CartVM();
            cartVM.Product = new List<Product>();
            cartVM.CountProduct = new List<int>();
            cartVM.CartId = new List<int>();
           
            IEnumerable<Cart> cartList = _db.Cart;
            int sum = 0;
            foreach (Cart cart in cartList)
            {
                sum += cart.Total * cart.Count;
                cartVM.CountProduct.Add(cart.Count) ;
                cartVM.CartId.Add(cart.Id);
            }
            
            foreach(Cart cart in cartList)
            {
                cartVM.Product.Add(_db.Product.Where(u => u.Id == cart.IdProduct).First());
            }
            cartVM.Total = sum;
            return View(cartVM);
        }
        public IActionResult Remove(int id)
        {
            var obj = _db.Cart.Find(id);
            _db.Cart.Remove(obj);
            _db.SaveChanges();
            return Redirect("~/Cart/Index/");
        }
        public VirtualFileResult PrintNal()
        {
            CartVM cartVM = new CartVM();
            cartVM.Product = new List<Product>();
            cartVM.CountProduct = new List<int>();
            int i = 0;
            IEnumerable<Cart> cartList = _db.Cart;
            int sum = 0;
            foreach (Cart cart in cartList)
            {
                sum += cart.Total * cart.Count;
                cartVM.CountProduct.Add(cart.Count);
            }
            
            foreach (Cart cart in cartList)
            {
                cartVM.Product.Add(_db.Product.Where(u => u.Id == cart.IdProduct).First());
            }
            cartVM.Total = sum;

            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();
            page.Size = PageSize.A5;

            XGraphics gfx = XGraphics.FromPdfPage(page);

            double fontHeight = 30;
            XFont font = new XFont("Times New Roman", fontHeight, XFontStyle.BoldItalic);

            double y = 0;
            int lineCount = 0;
            double linePadding = 10;

            XRect rect = new XRect(0, y, page.Width, fontHeight);
            gfx.DrawString("", font,
                           XBrushes.Black, rect, XStringFormats.Center);
            lineCount++;
            y += fontHeight;

            gfx.DrawString(DateTime.Now.ToString(), font,
                           XBrushes.Black, rect, XStringFormats.Center);
            lineCount++;
            y += fontHeight;

            rect = new XRect(0, y, page.Width, fontHeight);
            gfx.DrawString("Кассир:" + User.Identity.Name,
             font, XBrushes.Black,rect, XStringFormats.TopLeft);
            lineCount++;
            y += fontHeight;

            double topY = y - (lineCount * fontHeight) - linePadding;

            gfx.DrawLine(XPens.Black, 0, y, page.Width, y);
            

            foreach (var prod in cartVM.Product)
            {
                rect = new XRect(0, y, page.Width, fontHeight);
                gfx.DrawString(prod.Name + " X"+cartVM.CountProduct[i]+"  :"+prod.Cost* cartVM.CountProduct[i]+" Рублей",
            font, XBrushes.Black, rect, XStringFormats.TopLeft);
                lineCount++;
                y += fontHeight;
                i++;
            }

            gfx.DrawLine(XPens.Black, 0, y, page.Width, y);
            
            rect = new XRect(0, y, page.Width, fontHeight);
            gfx.DrawString("Сумма:"+ cartVM.Total+" Рублей   Наличка",
             font, XBrushes.Black, rect, XStringFormats.TopLeft);

            document.Save("wwwroot/doc/Check.pdf");
            var filepath = Path.Combine("~/doc", "Check.pdf");
            return File(filepath, "application/pdf", "Check.pdf");
        }

        public VirtualFileResult PrintBeznal()
        {
            CartVM cartVM = new CartVM();
            cartVM.Product = new List<Product>();
            cartVM.CountProduct = new List<int>();
            int i = 0;
            IEnumerable<Cart> cartList = _db.Cart;
            int sum = 0;
            foreach (Cart cart in cartList)
            {
                sum += cart.Total * cart.Count;
                cartVM.CountProduct.Add(cart.Count);
            }

            foreach (Cart cart in cartList)
            {
                cartVM.Product.Add(_db.Product.Where(u => u.Id == cart.IdProduct).First());
            }
            cartVM.Total = sum;

            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();
            page.Size = PageSize.A5;

            XGraphics gfx = XGraphics.FromPdfPage(page);

            double fontHeight = 30;
            XFont font = new XFont("Times New Roman", fontHeight, XFontStyle.BoldItalic);

            double y = 0;
            int lineCount = 0;
            double linePadding = 10;

            XRect rect = new XRect(0, y, page.Width, fontHeight);
            gfx.DrawString("", font,
                           XBrushes.Black, rect, XStringFormats.Center);
            lineCount++;
            y += fontHeight;

            gfx.DrawString(DateTime.Now.ToString(), font,
                           XBrushes.Black, rect, XStringFormats.Center);
            lineCount++;
            y += fontHeight;

            rect = new XRect(0, y, page.Width, fontHeight);
            gfx.DrawString("Кассир:" + User.Identity.Name,
             font, XBrushes.Black, rect, XStringFormats.TopLeft);
            lineCount++;
            y += fontHeight;

            double topY = y - (lineCount * fontHeight) - linePadding;

            gfx.DrawLine(XPens.Black, 0, y, page.Width, y);

            foreach (var prod in cartVM.Product)
            {
                rect = new XRect(0, y, page.Width, fontHeight);
                gfx.DrawString(prod.Name + " X" + cartVM.CountProduct[i] + "  :" + prod.Cost * cartVM.CountProduct[i] + " Рублей",
            font, XBrushes.Black, rect, XStringFormats.TopLeft);
                lineCount++;
                y += fontHeight;
                i++;
            }

            gfx.DrawLine(XPens.Black, 0, y, page.Width, y);

            rect = new XRect(0, y, page.Width, fontHeight);
            gfx.DrawString("Сумма:" + cartVM.Total + " Рублей   Картой",
             font, XBrushes.Black, rect, XStringFormats.TopLeft);

            document.Save("wwwroot/doc/Check.pdf");
            var filepath = Path.Combine("~/doc", "Check.pdf");
            return File(filepath, "application/pdf", "Check.pdf");
        }
    }
}
