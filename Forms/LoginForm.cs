using System;
using System.Drawing;
using System.Windows.Forms;
using SmartLibrary.DataAccess;

namespace SmartLibrary.Forms
{
    public class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblStatus;

        public LoginForm()
        {
            Text = "Smart Library - Login";
            Size = new Size(480, 400);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.FromArgb(15, 23, 42);

            Panel panelMain = new Panel();
            panelMain.Size = new Size(360, 300);
            panelMain.Location = new Point(60, 40);
            panelMain.BackColor = Color.FromArgb(30, 41, 59);
            Controls.Add(panelMain);

            Label lblTitle = new Label();
            lblTitle.Text = "Smart Library";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(56, 189, 248);
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(360, 45);
            lblTitle.Location = new Point(0, 15);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            panelMain.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Library Management System";
            lblSubtitle.Font = new Font("Segoe UI", 10);
            lblSubtitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblSubtitle.AutoSize = false;
            lblSubtitle.Size = new Size(360, 25);
            lblSubtitle.Location = new Point(0, 55);
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            panelMain.Controls.Add(lblSubtitle);

            Label lblUser = new Label();
            lblUser.Text = "Username";
            lblUser.Font = new Font("Segoe UI", 9);
            lblUser.ForeColor = Color.FromArgb(203, 213, 225);
            lblUser.Location = new Point(40, 100);
            lblUser.AutoSize = true;
            panelMain.Controls.Add(lblUser);

            txtUsername = new TextBox();
            txtUsername.Size = new Size(280, 30);
            txtUsername.Location = new Point(40, 122);
            txtUsername.Font = new Font("Segoe UI", 11);
            txtUsername.BackColor = Color.FromArgb(51, 65, 85);
            txtUsername.ForeColor = Color.White;
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Text = "admin";
            panelMain.Controls.Add(txtUsername);

            Label lblPass = new Label();
            lblPass.Text = "Password";
            lblPass.Font = new Font("Segoe UI", 9);
            lblPass.ForeColor = Color.FromArgb(203, 213, 225);
            lblPass.Location = new Point(40, 160);
            lblPass.AutoSize = true;
            panelMain.Controls.Add(lblPass);

            txtPassword = new TextBox();
            txtPassword.Size = new Size(280, 30);
            txtPassword.Location = new Point(40, 182);
            txtPassword.Font = new Font("Segoe UI", 11);
            txtPassword.BackColor = Color.FromArgb(51, 65, 85);
            txtPassword.ForeColor = Color.White;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.PasswordChar = '*';
            panelMain.Controls.Add(txtPassword);

            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Size = new Size(280, 40);
            btnLogin.Location = new Point(40, 230);
            btnLogin.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnLogin.BackColor = Color.FromArgb(37, 99, 235);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += BtnLogin_Click;
            panelMain.Controls.Add(btnLogin);

            lblStatus = new Label();
            lblStatus.Text = "";
            lblStatus.Font = new Font("Segoe UI", 9);
            lblStatus.ForeColor = Color.FromArgb(239, 68, 68);
            lblStatus.AutoSize = false;
            lblStatus.Size = new Size(280, 20);
            lblStatus.Location = new Point(40, 275);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            panelMain.Controls.Add(lblStatus);

            AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblStatus.Text = "Username and password are required.";
                return;
            }
            try
            {
                UserRepository repo = new UserRepository();
                var user = repo.Login(txtUsername.Text.Trim(), txtPassword.Text);
                if (user != null)
                {
                    SessionManager.CurrentUser = user;
                    Hide();
                    MainForm mainForm = new MainForm();
                    mainForm.FormClosed += delegate { Close(); };
                    mainForm.Show();
                }
                else
                {
                    lblStatus.Text = "Invalid username or password!";
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection error:\n\n" + ex.Message,
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
