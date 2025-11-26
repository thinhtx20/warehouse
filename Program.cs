using Inventory_manager;
using Inventory_manager.Models;

//using Inventory_manager.Models;
using Inventory_manager.Utili;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows.Forms;

internal static class Program
{
	[STAThread]
	static void Main()
	{
		// 1. Enable visual styles
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		// Scaffold-DbContext "Server=vantrong\SQLEXPRESS;Database=warehouses_manager;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force

		//// 2. Seed tài khoản admin nếu chưa có
		using (var db = new WarehousesManagerContext())
		{
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
				db.MaterialCategories.AddAsync(new MaterialCategory 
				{
					CategoryName = "Danh mục 1",
					
				});
				db.MaterialCategories.AddAsync(new MaterialCategory
				{
					CategoryName = "Danh mục 2",

				});
			}
			db.SaveChanges();
		}

		//// 3. Chạy LoginForm
		Application.Run(new LoginForm());
	}
}
