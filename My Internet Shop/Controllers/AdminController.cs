using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInternetShop.Models;
using MyInternetShop.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace MyInternetShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        public ApplicationContext db;
        public AdminController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.Categories = await db.Categories.ToListAsync();
            return View(db.Products.ToList());
        }

        public async Task<IActionResult> CategoryAsync()
        {
            return View(await db.Categories.ToListAsync());
        }

        [HttpGet]
        public IActionResult AddCategory() => View();

        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
               Category category = new Category { CategoryName = model.CategoryName };
               var result = db.Categories.Add(category);
               await db.SaveChangesAsync();

               return RedirectToAction("Category");
            }
            return View(model);
        }

        public async Task<IActionResult> EditCategory(int? id)
        {
            if (id != null)
            {
                Category category = await db.Categories.FirstOrDefaultAsync(p => p.Id == id);
                if (category != null)
                    return View(category);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Update(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Category");
            }
            return View(category);
        }

        public IActionResult AddProduct()
        {
            List<Category> categories = db.Categories.ToList();
            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            Image img = null;

            if (ModelState.IsValid)
            {
                if(Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];

                    img = new Image();
                    img.ImageTitle = file.FileName;

                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    img.ImageData = ms.ToArray();

                    ms.Close();
                    ms.Dispose();

                    await db.Images.AddAsync(img);
                    db.SaveChanges();
                }

                Product product;

                if (img != null)
                {
                    product = new Product { ProductName = model.ProductName, ImageId = img.Id, CategoryId = model.CategoryId, Price = model.Price, Quantity = model.Quantity, Description = model.Description };
                }

                else
                {
                    product = new Product { ProductName = model.ProductName, CategoryId = model.CategoryId, Price = model.Price, Quantity = model.Quantity, Description = model.Description };
                }
                
                db.Products.Add(product);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            if (ModelState["Price"].Errors.Any())
                ModelState["Price"].Errors[0] = new ModelError("Некорректный ввод");

            if (ModelState["Quantity"].Errors.Any() && ModelState["Quantity"].Errors[0].ErrorMessage.StartsWith("The"))
                ModelState["Quantity"].Errors[0] = new ModelError("Некорректный ввод");

            ViewBag.Categories = await db.Categories.ToListAsync();

            return View(model);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product, int? ImageId, bool ToDelete)
        {
            if (ModelState.IsValid)
            {
                Image img = null;

                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];

                    img = new Image();
                    img.ImageTitle = file.FileName;

                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    img.ImageData = ms.ToArray();

                    ms.Close();
                    ms.Dispose();

                    db.Images.Add(img);
                    db.SaveChanges();

                    product.ImageId = img.Id;
                    db.Products.Update(product);
                    db.SaveChanges();

                    //Deleting old image                  
                    if (ImageId != null)
                    {
                        Image OldImage = db.Images.FirstOrDefault(i => i.Id == ImageId);
                        db.Images.Remove(OldImage);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }

                else
                {
                    if (ImageId != null & ToDelete)
                    {
                        product.ImageId = null;
                        db.Products.Update(product);
                        db.SaveChanges();

                        Image OldImage = db.Images.FirstOrDefault(i => i.Id == ImageId);
                        db.Images.Remove(OldImage);
                        db.SaveChanges();
                    }

                    else
                    {
                        db.Products.Update(product);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Categories = db.Categories.ToList();

            return View(product);
        }

        public async Task<IActionResult> EditProduct(int? id, string returnurl)
        {
            if (id != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    Image image = db.Images.FirstOrDefault(i => i.Id == product.ImageId);

                    if (image != null)
                        ViewBag.ImageTitle = image.ImageTitle; 

                    ViewBag.Categories = db.Categories.ToList();
                    ViewBag.back = returnurl;
                    return View(product);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                
                if (product != null)
                {
                    Image image = db.Images.FirstOrDefault(i => i.Id == product.ImageId);
                    db.Products.Remove(product);

                    if(image != null)
                    db.Images.Remove(image);

                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult DeleteCategory(int? id)
        {
            try
            {
                if (id != null)
                {
                    Category category = db.Categories.FirstOrDefault(p => p.Id == id);

                    if (category != null)
                    {
                        if(category.CategoryName == "No Category")
                        {
                            ViewBag.Error = "Вы не можете удалить эту категорию";
                            return View("Category", db.Categories.ToList());
                        }

                        db.Categories.Remove(category);
                        db.SaveChanges();
                        return RedirectToAction("Category");
                    }
                }
                return NotFound();
            }

            catch (DbUpdateException)
            {
                ViewBag.Error = "Невозможно удалить категорию, в данной категории есть товары";
                return View("Category", db.Categories.ToList());
            }
        }

        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (id != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);

                Image img = db.Images.FirstOrDefault(img => img.Id == product.ImageId);

                if(img != null)
                {
                  string imageBase64Data = Convert.ToBase64String(img.ImageData);
                  string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                  ViewBag.ImageDataUrl = imageDataURL;
                }

                ViewBag.Categories = db.Categories.ToListAsync();

                return View(product);
            }
            return NotFound();
        }

        public IActionResult Orders()
        {
            var orders = db.Orders.Include(o => o.OrderEntities).ThenInclude(e => e.Product).Include(e => e.User).OrderByDescending(i => i.Date).ToList();

            ViewBag.tab = "Все заказы";

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrderAsync(int OrderId)
        {
            Order order = db.Orders.FirstOrDefault(o => o.Id == OrderId);

            if (order == null)
                return BadRequest();

            db.Entry(order).Collection("OrderEntities").Load();

            foreach (OrderEntity o in order.OrderEntities)
            {
                db.Entry(o).Reference("Product").Load();
                o.Product.Quantity -= o.Count;
            }

            order.Status = Status.Confirmed;
            await db.SaveChangesAsync();

            return new JsonResult(new { id = order.Id, status = "Confirmed"});
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrderAsync(int OrderId)
        {
            Order order = db.Orders.FirstOrDefault(o => o.Id == OrderId);

            if (order == null)
                return BadRequest();

            db.Entry(order).Collection("OrderEntities").Load();

            foreach (OrderEntity o in order.OrderEntities)
            {
                db.Entry(o).Reference("Product").Load();
                o.Product.Quantity += o.Count;
            }

            order.Status = Status.Cancelled;
            await db.SaveChangesAsync();

            return new JsonResult(new { id = order.Id, status = Status.Cancelled });
        }

        public async Task<IActionResult> NewOrdersAsync()
        {
            var orders = db.Orders.Include(o => o.OrderEntities).ThenInclude(e => e.Product).Include(e => e.User).OrderByDescending(i => i.Date).ToList().Where(o => o.Status == Status.New || o.Status == Status.Waiting);

            foreach (var order in orders)
            {
                if (order.Status == Status.New)
                    order.Status = Status.Waiting;
            }
            await db.SaveChangesAsync();
            ViewBag.tab = "Новые";

            return View("Orders", orders);
        }

        public IActionResult ConfirmedOrders()
        {
            var orders = db.Orders.Include(o => o.OrderEntities).ThenInclude(e => e.Product).Include(e => e.User).OrderByDescending(i => i.Date).ToList().Where(o => o.Status == Status.Confirmed);

            ViewBag.tab = "Подтвержденные";

            return View("Orders", orders);
        }

        public IActionResult CancelledOrders()
        {
            var orders = db.Orders.Include(o => o.OrderEntities).ThenInclude(e => e.Product).Include(e => e.User).OrderByDescending(i => i.Date).ToList().Where(o => o.Status == Status.Cancelled || o.Status == Status.CancelledByUser);

            ViewBag.tab = "Отмененные";

            return View("Orders", orders);
        }

        public IActionResult GetNewOrdersCount()
        {
            int count = db.Orders.Where(o => o.Status == Status.New).Count();

            return new JsonResult(count);
        }

        public async Task<IActionResult> SelectCategoryAsync(int Id)
        {
            ViewBag.CategoryName = db.Categories.FirstOrDefaultAsync(c => c.Id == Id).Result.CategoryName;
            ViewBag.Categories = await db.Categories.ToListAsync();

            List<Product> products = db.Products.Where(p => p.CategoryId == Id).ToList();
            
            return View("Index", products);
        }
    }
}
