
using Inventory_manager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_manager
{
	public partial class IssueForm : Form
	{
		private User _currentUser;
		public IssueForm(User user)
		{
			_currentUser = user;
			InitializeComponent();
		}
	}
}
