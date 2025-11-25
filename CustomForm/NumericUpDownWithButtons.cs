using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_manager.CustomForm
{
	public partial class NumericUpDownWithButtons : UserControl
	{
		public NumericUpDownWithButtons()
		{
			InitializeComponent();
			try
			{

				btnPlus.Click += (s, e) =>
				{
					if (numericUpDown.Value < numericUpDown.Maximum)
						numericUpDown.Value++;
				};

				btnMinus.Click += (s, e) =>
				{
					if (numericUpDown.Value > numericUpDown.Minimum)
						numericUpDown.Value--;
				};



			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public decimal Value
		{
			get => numericUpDown.Value;
			set => numericUpDown.Value = value;
		}
	}

}
