using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyInternetShop.Models;
using MyInternetShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MyInternetShop.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private ApplicationContext db;
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;

        public UsersController(UserManager<User> userManager, ApplicationContext context, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            db = context;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());

        public async Task<IActionResult> Edit(string name)
        {
            User user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, UserName = user.UserName, Email = user.Email, Year = user.Year };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.Year = model.Year;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        ViewBag.Categories = await db.Categories.ToListAsync();
                        await _signInManager.SignInAsync(user, false);
                        ViewBag.UserName = user.UserName;
                        return (RedirectToAction("Index", "Home"));
                        //return View("~/Views/Home/Index.cshtml");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var orders = db.Orders.Include(o => o.OrderEntities).Where(o => o.UserId == id);
                
                foreach(Order o in orders)
                {
                    var orderEntities = o.OrderEntities.Where(oe => oe.OrderId == o.Id);
                    db.RemoveRange(orderEntities);
                    db.Remove(o);
                }
              
                db.SaveChanges();
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, UserName = user.UserName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                        await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            if (error.Description.StartsWith("Incorrect"))
                                error.Description = "Неверный пароль";

                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Orders()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            var orders = db.Orders.Where(o => o.UserId == user.Id).Include(o => o.OrderEntities).ThenInclude(e => e.Product).OrderByDescending(i => i.Date).ToList();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrderAsync(int OrderId)
        {
            Order order = db.Orders.FirstOrDefault(o => o.Id == OrderId);

            if (order == null)
                return BadRequest();

            //db.Entry(order).Collection("OrderEntities").Load();

            //foreach (OrderEntity o in order.OrderEntities)
            //{
            //    db.Entry(o).Reference("Product").Load();
            //    o.Product.Quantity += o.Count;
            //}

            order.Status = Status.CancelledByUser;
            await db.SaveChangesAsync();

            return new JsonResult(new { id = order.Id, status = Status.CancelledByUser });
        }

        [HttpPost]
        public async Task<IActionResult> ResumeOrderAsync(int OrderId)
        {
            Order order = db.Orders.FirstOrDefault(o => o.Id == OrderId);

            if (order == null)
                return BadRequest();

            db.Entry(order).Collection("OrderEntities").Load();

            foreach (OrderEntity o in order.OrderEntities)
            {
                db.Entry(o).Reference("Product").Load();

                if(o.Product.Quantity < o.Count)
                {
                    return new JsonResult(new { message = "К сожалению некоторых товаров в этом заказе уже не достаточно" +
                        "\nНевозможно восстановить заказ", ProductName = o.Product.ProductName });
                }
                //o.Product.Quantity -= o.Count;
            }

            order.Status = Status.New;
            await db.SaveChangesAsync();

            return new JsonResult(new { id = order.Id, status = Status.Waiting });
        }
    }
}

