using Inventory_manager.Models;
using Inventory_manager.Utili;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_manager.Services
{
	public class AuthService
	{
		private readonly WarehousesManagerContext _context;

		public AuthService()
		{
			_context = new WarehousesManagerContext();
		}

		public User? Login(string username, string password)
		{
			string hash = Utils.HashPassword(password);

			return _context.Users.FirstOrDefault(u => u.Username == username && u.PasswodHash == hash && u.IsActive == true);
		}
	}

}
