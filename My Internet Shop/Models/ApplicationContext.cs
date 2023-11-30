using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyInternetShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyInternetShop.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Image> Images { get; set; }
        public DbSet<CartEntity> CartEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasIndex(c => c.CategoryName).IsUnique();
            modelBuilder.Entity<Category>().HasData(new Category { Id = 1, CategoryName = "No Category" });
            modelBuilder.Entity<Category>().HasData(new Category { Id = 2, CategoryName = "Power bank" });
            modelBuilder.Entity<Category>().HasData(new Category { Id = 3, CategoryName = "Flash drives" });


            modelBuilder.Entity<Product>().HasData(new Product { Id = 2,
             ProductName = "Xiaomi Power bank 20000", 
             CategoryId = 2,
             Price = 7000,
             Quantity = 3,
             Description = "Благодаря высокой емкости 20 000 мАч и трем портам для зарядки,\nвключая порт USB Type - C с поддержкой" +
             "двусторонней зарядки с мощностью до 45 Вт\n Xiaomi 20000mAh 50W PD Power Bank 3 Pro способен обеспечивать напряжение на выходе:\n 5 В / 2.4 А, 9 В / 2 А или 12 В / 1.5 А - при раздельном использовании\n и 5 В / 3 А - при одновременном использовании.\n Порт USB Type - C может быть использован как для зарядки самого внешнего\n аккумулятора Xiaomi 20000mAh 50W PD Power Bank 3 Pro\n так и для зарядки устройств с мощностью до 45 Вт.\n\n" +

            "Коротко о товаре\n" +
            "Цвет товара:	белый\n" +
            "Тип упаковки:	коробка\n" +
            "Совместимость:	быстрая зарядка\n\n" +
            "Особенности:	зарядка двух устрйств,\n" +
                            "индикатор заряда,\n" +
                            "Быстрая зарядка Quick Charge,\n" +
                            "Быстрая зарядка Quick Charge" });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 3,
                ProductName = "Netac Usb flash drive",
                CategoryId = 3,
                Quantity = 3,
                Price = 2000,
                Description = "Основные характеристики:\n\n" +
                "Бренд: NETAC\n" +
                "Модель: U182\n" +
                "Объем: 256 ГБ\n" +
                "Интерфейс: USB3.0\n" +
                "Разъем: USB А, выдвижной" 
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 4,
                ProductName = "Xiaomi PowerBank ZMIQB816",
                CategoryId = 2,
                Quantity = 3,
                Price = 4950,
                Description = "Основные характеристики:\n\n" +
                "Батарея:  Li-Ion, 10000мAч;\n" +
                "Особенности:  индикатор уровня заряда, быстрая зарядка,\n" +
                "Материал корпуса:  пластик;\n" +
                "В комплекте:  кабель для зарядки\n" +
                "Сила тока на выходе:  3А +3А +2.4А"
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 5,
                ProductName = "Флешка USB Silicon Power Marvel M01 32ГБ, USB3.0, синий",
                CategoryId = 3,
                Quantity = 3,
                Price = 460,
                Description = "Совместимая со всеми современными операционными системами\n флешка USB SILICON POWER Marvel M01\n представляет собой устройство Plug and play, которое не нуждается в дополнительных источниках питания и сразу же\n  после подключения к компьютеру готово к работе.\n Благодаря поддержке интерфейса USB версии 3.0\n  обеспечивается высокая пропускная способность. Флешка USB SILICON POWER Marvel M01 подходит для хранения данных разного формата и\nоснащена для этого 32 Гб памяти. При подключении к компьютеру и во время передачи информации светится LED-индикатор активности.\n  Данная модель выполнена в компактном алюминиевом корпусе,\n который защищен от механических\n повреждений, и дополнена пластиковым колпачком, предотвращающим вероятность засорения разъема.\n Такие характеристики позволяют всегда иметь ее при себе.\n\n" +
                
                
                "Основные характеристики:\n\n" +
                "Объем:  32ГБ\n" +
                "Интерфейс:  USB3.0;\n" +
                "Разъем:  USB А, закрывается колпачком\n" +
                "Материал корпуса:  алюминий;"
            });


            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 6,
                ProductName = "Флешка USB Kingston DataTraveler Exodia M 64ГБ, USB3.0, черный и синий",
                CategoryId = 3,
                Quantity = 3,
                Price = 490,
                Description = "Основные характеристики:\n\n" +
                "Объем: 64 ГБ\n" +
                "Интерфейс: USB3.0\n" +
                "Разъем: USB А, выдвижной\n" +
                "Материал корпуса:  пластик;"
            });

            modelBuilder.Entity<CartEntity>().HasKey(ce => new { ce.UserId, ce.ProductId });
            modelBuilder.Entity<OrderEntity>().HasKey(oe => new { oe.UserId, oe.ProductId, oe.OrderId });
        }
    }
}
