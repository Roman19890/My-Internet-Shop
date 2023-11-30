using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MyInternetShop.Models;
using System.IO;
using MyInternetShop.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MyInternetShop.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        private readonly ILogger<HomeController> _logger;
        UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context, UserManager<User> userManager)
        {
            _logger = logger;
            db = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        public async Task<IActionResult> SelectCategory(int Id)
        {
            ViewBag.CategoryName = db.Categories.FirstOrDefaultAsync(c => c.Id == Id).Result.CategoryName;
            ViewBag.Categories = await db.Categories.ToListAsync();

            var products = db.Products.Where(p => p.CategoryId == Id).ToList();

            List<SelectProductViewModel> ReturnedProducts = new List<SelectProductViewModel>();

            foreach (var p in products)
            {
                string imageDataURL = null;

                if (p.ImageId != null)
                {
                    Image img = db.Images.FirstOrDefault(img => img.Id == p.ImageId);

                    if (img != null)
                    {
                        string imageBase64Data = Convert.ToBase64String(img.ImageData);
                        imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                    }
                }
                SelectProductViewModel product = new SelectProductViewModel { Id = p.Id, ProductName = p.ProductName, ImageDataUrl = imageDataURL };
                ReturnedProducts.Add(product);
            }
            //ViewBag.CategoryName = category.CategoryName;
            return View(ReturnedProducts);
        }

        public async Task<IActionResult> ProductPageAsync(int Id, string returnurl)
        {
            Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == Id);

            Image img = db.Images.FirstOrDefault(img => img.Id == product.ImageId);

            if (img != null)
            {
                string imageBase64Data = Convert.ToBase64String(img.ImageData);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                ViewBag.ImageDataUrl = imageDataURL;
            }
            ViewBag.back = returnurl;
            return View(product);
        }

        public async Task<IActionResult> SearchAsync(string SearchString)
        {
            ViewBag.Categories = await db.Categories.ToListAsync();

            var products = db.Products.Where(p => p.ProductName.Contains(SearchString)).ToList();

            List<SelectProductViewModel> ReturnedProducts = new List<SelectProductViewModel>();

            foreach (var p in products)
            {
                string imageDataURL = null;

                if (p.ImageId != null)
                {
                    Image img = db.Images.FirstOrDefault(img => img.Id == p.ImageId);

                    if (img != null)
                    {
                        string imageBase64Data = Convert.ToBase64String(img.ImageData);
                        imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                    }
                }

                SelectProductViewModel product = new SelectProductViewModel { Id = p.Id, ProductName = p.ProductName, ImageDataUrl = imageDataURL };
                ReturnedProducts.Add(product);
            }
            return View(ReturnedProducts);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCartAsync(int ProductId, string UserName)
        {
            Product product = db.Products.FirstOrDefault(p => p.Id == ProductId);
            User user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                return NotFound();
            }

            CartEntity cartEntity = db.CartEntities.FirstOrDefault(e => e.UserId == user.Id & e.ProductId == ProductId);

            if (cartEntity != null)
            {
                cartEntity.Count++;

                if(cartEntity.Count > product.Quantity)
                {
                    return new JsonResult(new {message = "Товара недостаточно" });
                }

                else 
                db.CartEntities.Update(cartEntity);
            }

            else
            {
                if (product.Quantity == 0)
                {
                    return new JsonResult(new { message = "Товара недостаточно" });
                }

                cartEntity = new CartEntity();

                cartEntity.ProductId = ProductId;
                cartEntity.UserId = user.Id;
                cartEntity.Count = 1;
                db.CartEntities.Add(cartEntity);
            }
           await db.SaveChangesAsync();

            var CartItems = db.CartEntities.Include(p => p.Product).Where(e => e.UserId == user.Id);

            int count = 0;

            foreach (var item in CartItems)
            {
                count += item.Count;
            }
            return new JsonResult(count);
        }

        public async Task<IActionResult> GetCartProductsCountAsync()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            var CartItems = db.CartEntities.Include(p => p.Product).Where(e => e.UserId == user.Id);

            int count = 0;

            foreach (var item in CartItems)
            {
                count += item.Count;
            }
            return new JsonResult(count);
        }

        public async Task<IActionResult> ShoppingCartAsync()
        {
            if (User.Identity.Name == null)
            {
                ViewBag.Categories = db.Categories.ToList();
                return View("Index");
            }

            else
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }

                var CartItems = db.CartEntities.Include(p => p.Product).Where(e => e.UserId == user.Id).ToList();
                List<CartItemViewModel> ReturnedCartItems = new List<CartItemViewModel>();

                foreach (var item in CartItems)
                {
                    string imageDataURL = null;

                    if (item.Product.ImageId != null)
                    {
                        Image img = db.Images.FirstOrDefault(img => img.Id == item.Product.ImageId);

                        if (img != null)
                        {
                            string imageBase64Data = Convert.ToBase64String(img.ImageData);
                            imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                        }
                    }
                    ReturnedCartItems.Add(new CartItemViewModel() { UserId = user.Id, ProductId = item.ProductId, ProductName = item.Product.ProductName, Quantity = item.Count, Sum = item.Count * item.Product.Price, ImageDataUrl = imageDataURL, ProductLeftOver = item.Product.Quantity });
                }
                return View(ReturnedCartItems);
            }
        }

        public async Task<IActionResult> DeleteFromCartAsync(int id, string name)
        {
            User user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return NotFound();
            }

            CartEntity entity = db.CartEntities.FirstOrDefault(e => e.ProductId == id & e.UserId == user.Id);

            db.CartEntities.Remove(entity);
            db.SaveChanges();

            return RedirectToAction("ShoppingCart");
        }

        public async Task<IActionResult> OrderAsync()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            var CartItems = db.CartEntities.Include(p => p.Product).Where(e => e.UserId == user.Id).ToList();

            List<Product> products = new List<Product>();
            bool failed = false;

            foreach (var item in CartItems)
            {
                if(item.Product.Quantity < item.Count)
                {
                    failed = true;
                    products.Add(item.Product);
                }
            }

            if(failed)
            {
                ViewBag.Products = products;
                return View("OrderFailure");
            }

            List<OrderEntity> OrderEntities = new List<OrderEntity>();
            Order order = new Order() { UserId = user.Id, Date = DateTime.Now, Status = Status.New };

            decimal? cost = 0;

            foreach (var item in CartItems)
            {
                OrderEntity orderEntity = new OrderEntity() { UserId = user.Id, ProductId = item.ProductId, OrderId = order.Id, Count = item.Count };
                order.OrderEntities.Add(orderEntity);
                cost += item.Product.Price * item.Count;
                //item.Product.Quantity -= item.Count;
                //db.Products.Update(item.Product);
            }

            db.Orders.Add(order);
            db.SaveChanges();

            db.CartEntities.RemoveRange(CartItems);
            order.Cost = cost;

            await db.SaveChangesAsync();

            return View();
        }

        public IActionResult ChangeQuantity(string UserId, int ProductId, int CountId, int Count)
        {
            CartEntity entity = db.CartEntities.FirstOrDefault(e => e.UserId == UserId & e.ProductId == ProductId);
            db.Entry(entity).Reference("Product").Load();

            if(Count <= 0)
            {
                int RemoveQuantity = entity.Count;
                decimal? RemoveSum = RemoveQuantity * entity.Product.Price;

                db.CartEntities.Remove(entity);
                db.SaveChangesAsync();

                return new JsonResult(new { remove = true, CountId, RemoveQuantity, RemoveSum });
            }

            if(entity.Product.Quantity < Count)
            {
                return new JsonResult(new { Error =  "Данного товара недостаточно в наличии"});
            }

            int Quantity = entity.Count;

            int CountDifference = Count - Quantity;

            decimal? Difference = CountDifference * entity.Product.Price;

            entity.Count = Count;
            db.SaveChangesAsync();

            return new JsonResult(new { Difference, CountId, CountDifference });
        }

        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult Delivery()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
