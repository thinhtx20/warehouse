using Inventory_manager.Models;
using Inventory_manager.Utili;
using OfficeOpenXml;
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
            // ✅ EPPlus 8+ - set license ĐÚNG CÁCH
            ExcelPackage.License.SetNonCommercialPersonal("Inventory_manager");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SeedData();
            Application.Run(new LoginForm());
        }

        private static void SeedData()
        {
            using var db = new WarehousesManagerContext();

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

            if (!db.MaterialCategories.Any())
            {
                db.MaterialCategories.AddRange(
                    new MaterialCategory { CategoryName = "Danh mục 1" },
                    new MaterialCategory { CategoryName = "Danh mục 2" }
                );
            }

            db.SaveChanges();
        }
    }
}
