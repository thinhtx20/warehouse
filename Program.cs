using Inventory_manager.Models;
using Inventory_manager.Utili;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Inventory_manager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Seed dữ liệu ban đầu
            SeedData();

            // Chạy form login
            Application.Run(new LoginForm());
        }

        private static void SeedData()
        {
            using var db = new WarehousesManagerContext();

            // 1. Seed admin
            if (!db.Users.Any())
            {
                db.Users.Add(new User
                {
                    Username = "admin",
                    PasswodHash = Utils.HashPassword("123456"),
                    FullName = "admin",
                    Role = "admin",
                    IsActive = true
                });
            }

            // 2. Seed danh mục vật tư
            if (!db.MaterialCategories.Any())
            {
                db.MaterialCategories.AddRange(
                    new MaterialCategory
                    {
                        CategoryName = "Danh mục 1"
                    },
                    new MaterialCategory
                    {
                        CategoryName = "Danh mục 2"
                    }
                );
            }

            db.SaveChanges();
        }
    }
}
