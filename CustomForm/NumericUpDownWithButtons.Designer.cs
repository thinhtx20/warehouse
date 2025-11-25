namespace Inventory_manager.CustomForm
{
	partial class NumericUpDownWithButtons
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		public System.ComponentModel.IContainer components = null;
		public System.Windows.Forms.Button btnPlus;
		public System.Windows.Forms.Button btnMinus;
		public System.Windows.Forms.NumericUpDown numericUpDown;
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			SuspendLayout();
			// 
			// NumericUpDownWithButtons
			// 
			// Khởi tạo NumericUpDown
			numericUpDown = new NumericUpDown
			{
				Minimum = 0,
				Maximum = 100,
				Value = 0,
				Width = 60,
				Location = new System.Drawing.Point(30, 0)
			};

			// Khởi tạo nút +
			btnPlus = new Button
			{
				Text = "+",
				Width = 25,
				Height = numericUpDown.Height / 2,
				Location = new System.Drawing.Point(0, 0)
			};

			// Khởi tạo nút -
			btnMinus = new Button
			{
				Text = "-",
				Width = 25,
				Height = numericUpDown.Height / 2,
				Location = new System.Drawing.Point(0, numericUpDown.Height / 2)
			};
			// Thêm các control vào UserControl
			this.Controls.Add(numericUpDown);
			this.Controls.Add(btnPlus);
			this.Controls.Add(btnMinus);
			// Set kích thước UserControl
			this.Width = numericUpDown.Width + btnPlus.Width;
			this.Height = numericUpDown.Height;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Name = "NumericUpDownWithButtons";
			ResumeLayout(false);
		}

		#endregion
	}
}
