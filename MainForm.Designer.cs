using System.Windows.Forms;
namespace Inventory_manager
{
	partial class MainForm : Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
		private GroupBox grbMenu;
		private GroupBox grbMain;
		private BindingSource receiptRequestModelsBindingSource;
		private DataGridView dgvMaterials;
		private BindingSource materialResponeMessageBindingSource;
		private DataGridViewTextBoxColumn STT;
		private DataGridViewTextBoxColumn materialIdDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn materialNameDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn unitDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn categoryNameDataGridViewTextBoxColumn;
		private GroupBox groupBox1;
	}
}