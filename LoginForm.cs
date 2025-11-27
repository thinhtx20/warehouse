using Inventory_manager.Services;
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
	public partial class LoginForm : Form
	{
		private Label lblTitle;
		private Label lblUser;
		private Label lblPass;
		private TextBox txtUsername;
		private TextBox txtPassword;
		private Button btnLogin;
		private Button btnExit;

		public LoginForm()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			lblTitle = new Label();
			lblUser = new Label();
			txtUsername = new TextBox();
			lblPass = new Label();
			txtPassword = new TextBox();
			btnLogin = new Button();
			btnExit = new Button();
			SuspendLayout();
			// 
			// lblTitle
			// 
			lblTitle.AutoSize = true;
			lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
			lblTitle.Location = new Point(142, 20);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new Size(292, 32);
			lblTitle.TabIndex = 0;
			lblTitle.Text = "ĐĂNG NHẬP HỆ THỐNG";
			// 
			// lblUser
			// 
			lblUser.AutoSize = true;
			lblUser.Location = new Point(80, 77);
			lblUser.Name = "lblUser";
			lblUser.Size = new Size(110, 20);
			lblUser.TabIndex = 1;
			lblUser.Text = "Tên đăng nhập:";
			// 
			// txtUsername
			// 
			txtUsername.Location = new Point(210, 74);
			txtUsername.Name = "txtUsername";
			txtUsername.Size = new Size(259, 27);
			txtUsername.TabIndex = 2;
			// 
			// lblPass
			// 
			lblPass.AutoSize = true;
			lblPass.Location = new Point(80, 120);
			lblPass.Name = "lblPass";
			lblPass.Size = new Size(73, 20);
			lblPass.TabIndex = 3;
			lblPass.Text = "Mật khẩu:";
			// 
			// txtPassword
			// 
			txtPassword.Location = new Point(210, 117);
			txtPassword.Name = "txtPassword";
			txtPassword.PasswordChar = '*';
			txtPassword.Size = new Size(259, 27);
			txtPassword.TabIndex = 4;
			// 
			// btnLogin
			// 
			btnLogin.Location = new Point(142, 169);
			btnLogin.Name = "btnLogin";
			btnLogin.Size = new Size(110, 32);
			btnLogin.TabIndex = 5;
			btnLogin.Text = "Đăng nhập";
			btnLogin.Click += BtnLogin_Click;
			// 
			// btnExit
			// 
			btnExit.Location = new Point(322, 169);
			btnExit.Name = "btnExit";
			btnExit.Size = new Size(127, 35);
			btnExit.TabIndex = 6;
			btnExit.Text = "Thoát";
			btnExit.Click += BtnExit_Click;
			// 
			// LoginForm
			// 
			ClientSize = new Size(552, 249);
			Controls.Add(lblTitle);
			Controls.Add(lblUser);
			Controls.Add(txtUsername);
			Controls.Add(lblPass);
			Controls.Add(txtPassword);
			Controls.Add(btnLogin);
			Controls.Add(btnExit);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			Name = "LoginForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Đăng nhập hệ thống";
			ResumeLayout(false);
			PerformLayout();
		}

		// Button Login
		private void BtnLogin_Click(object sender, EventArgs e)
		{
			string user = txtUsername.Text.Trim();
			string pass = txtPassword.Text.Trim();
			//string user = "admin";
			//string pass = "123456";

			if (user == "" || pass == "")
			{
				MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!");
				return;
			}

			AuthService auth = new AuthService();
			var loginUser = auth.Login(user, pass);

			if (loginUser != null)
			{
				MessageBox.Show($"Đăng nhập thành công! Xin chào {loginUser.FullName}");

				var main = new MainForm(loginUser);
				main.Show();

				this.Hide();
			}
			else
			{
				MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
			}
		}

		// Button Exit
		private void BtnExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
