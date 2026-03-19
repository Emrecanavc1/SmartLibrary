using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SmartLibrary.DataAccess;
using SmartLibrary.Models;

namespace SmartLibrary.Forms
{
    public class MainForm : Form
    {
        private Panel panelSidebar;
        private Panel panelTopBar;
        private Panel panelContent;
        private Label lblPageTitle;
        private List<Button> navButtons = new List<Button>();
        private Button activeButton;
        private DataGridView dgvBooks;
        private DataGridView dgvMembers;

        private static Color SidebarBg = Color.FromArgb(15, 23, 42);
        private static Color ContentBg = Color.FromArgb(241, 245, 249);
        private static Color AccentBlue = Color.FromArgb(37, 99, 235);
        private static Color CardBg = Color.White;
        private static Color TextDark = Color.FromArgb(30, 41, 59);
        private static Color TextMuted = Color.FromArgb(100, 116, 139);

        public MainForm()
        {
            Text = "Smart Library - Library Management System";
            Size = new Size(1280, 780);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = ContentBg;
            MinimumSize = new Size(1000, 600);

            BuildSidebar();
            BuildTopBar();
            BuildContent();

            SetActiveNav(navButtons[0]);
            LoadDashboard();
        }

        private void BuildSidebar()
        {
            panelSidebar = new Panel();
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Width = 220;
            panelSidebar.BackColor = SidebarBg;
            Controls.Add(panelSidebar);

            Label lblLogo = new Label();
            lblLogo.Text = "Smart Library";
            lblLogo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblLogo.ForeColor = Color.FromArgb(56, 189, 248);
            lblLogo.AutoSize = false;
            lblLogo.Size = new Size(220, 60);
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            panelSidebar.Controls.Add(lblLogo);

            string[] pages = { "Dashboard", "Books", "Members", "Borrow Book", "Return Book", "Reports", "Settings" };
            int y = 80;
            foreach (string page in pages)
            {
                Button btn = new Button();
                btn.Text = "   " + page;
                btn.Tag = page;
                btn.Size = new Size(220, 42);
                btn.Location = new Point(0, y);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
                btn.BackColor = SidebarBg;
                btn.ForeColor = Color.FromArgb(203, 213, 225);
                btn.Font = new Font("Segoe UI", 11);
                btn.TextAlign = ContentAlignment.MiddleLeft;
                btn.Padding = new Padding(10, 0, 0, 0);
                btn.Cursor = Cursors.Hand;
                btn.Click += NavButton_Click;
                panelSidebar.Controls.Add(btn);
                navButtons.Add(btn);
                y += 46;
            }
            Label lblAdmin = new Label();
            lblAdmin.Text = "  ADMIN ACTIONS";
            lblAdmin.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblAdmin.ForeColor = Color.FromArgb(100, 116, 139);
            lblAdmin.Size = new Size(220, 25);
            lblAdmin.Location = new Point(0, y + 20);
            panelSidebar.Controls.Add(lblAdmin);

            Button btnAddBook = new Button();
            btnAddBook.Text = "   + Add Book";
            btnAddBook.Size = new Size(220, 38);
            btnAddBook.Location = new Point(0, y + 45);
            btnAddBook.FlatStyle = FlatStyle.Flat;
            btnAddBook.FlatAppearance.BorderSize = 0;
            btnAddBook.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
            btnAddBook.BackColor = Color.FromArgb(37, 99, 235);
            btnAddBook.ForeColor = Color.White;
            btnAddBook.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAddBook.TextAlign = ContentAlignment.MiddleLeft;
            btnAddBook.Padding = new Padding(10, 0, 0, 0);
            btnAddBook.Cursor = Cursors.Hand;
            btnAddBook.Click += delegate { ShowBookForm(null); };
            panelSidebar.Controls.Add(btnAddBook);

            Button btnAddMember = new Button();
            btnAddMember.Text = "   + Add Member";
            btnAddMember.Size = new Size(220, 38);
            btnAddMember.Location = new Point(0, y + 87);
            btnAddMember.FlatStyle = FlatStyle.Flat;
            btnAddMember.FlatAppearance.BorderSize = 0;
            btnAddMember.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
            btnAddMember.BackColor = Color.FromArgb(16, 185, 129);
            btnAddMember.ForeColor = Color.White;
            btnAddMember.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAddMember.TextAlign = ContentAlignment.MiddleLeft;
            btnAddMember.Padding = new Padding(10, 0, 0, 0);
            btnAddMember.Cursor = Cursors.Hand;
            btnAddMember.Click += delegate { ShowMemberForm(null); };
            panelSidebar.Controls.Add(btnAddMember);
        }

        private void BuildTopBar()
        {
            panelTopBar = new Panel();
            panelTopBar.Dock = DockStyle.Top;
            panelTopBar.Height = 60;
            panelTopBar.BackColor = Color.White;
            Controls.Add(panelTopBar);

            lblPageTitle = new Label();
            lblPageTitle.Text = "Dashboard";
            lblPageTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblPageTitle.ForeColor = TextDark;
            lblPageTitle.AutoSize = true;
            lblPageTitle.Location = new Point(20, 15);
            panelTopBar.Controls.Add(lblPageTitle);

            string info = SessionManager.CurrentUser != null
                ? SessionManager.CurrentUser.FullName + " (" + SessionManager.CurrentUser.Role + ")"
                : "";
            Label lblUser = new Label();
            lblUser.Text = info;
            lblUser.Font = new Font("Segoe UI", 10);
            lblUser.ForeColor = TextMuted;
            lblUser.AutoSize = true;
            lblUser.Location = new Point(400, 20);
            panelTopBar.Controls.Add(lblUser);

            Button btnLogout = new Button();
            btnLogout.Text = "Logout";
            btnLogout.Size = new Size(70, 30);
            btnLogout.Location = new Point(700, 15);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.BackColor = Color.FromArgb(239, 68, 68);
            btnLogout.ForeColor = Color.White;
            btnLogout.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.Click += delegate
            {
                if (MessageBox.Show("Are you sure you want to logout?", "Logout",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SessionManager.CurrentUser = null;
                    Close();
                }
            };
            panelTopBar.Controls.Add(btnLogout);
        }

        private void BuildContent()
        {
            panelContent = new Panel();
            panelContent.Dock = DockStyle.Fill;
            panelContent.BackColor = ContentBg;
            panelContent.AutoScroll = true;
            Controls.Add(panelContent);
            panelContent.BringToFront();
            panelTopBar.BringToFront();
        }

        private void SetActiveNav(Button btn)

        {
            if (activeButton != null)
            {
                activeButton.BackColor = SidebarBg;
                activeButton.ForeColor = Color.FromArgb(203, 213, 225);
            }
            activeButton = btn;
            btn.BackColor = AccentBlue;
            btn.ForeColor = Color.White;
        }

        private void NavButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            SetActiveNav(btn);
            string page = btn.Tag.ToString();
            panelContent.Controls.Clear();
            lblPageTitle.Text = page;

            if (page == "Dashboard") LoadDashboard();
            else if (page == "Books") LoadBooksPage();
            else if (page == "Members") LoadMembersPage();
            else if (page == "Borrow Book") LoadBorrowPage();
            else if (page == "Return Book") LoadReturnPage();
            else if (page == "Reports") LoadReportsPage();
            else if (page == "Settings") LoadSettingsPage();
        }

        // ==================== DASHBOARD ====================
        private void LoadDashboard()
        {
            panelContent.Controls.Clear();
            lblPageTitle.Text = "Dashboard";
            try
            {
                DashboardStats stats = new BorrowRepository().GetDashboardStats();
                int x = 20;
                AddStatCard("Total Books", stats.TotalBooks.ToString(), Color.FromArgb(37, 99, 235), x, 10); x += 220;
                AddStatCard("Total Members", stats.TotalMembers.ToString(), Color.FromArgb(16, 185, 129), x, 10); x += 220;
                AddStatCard("Borrowed", stats.ActiveBorrows.ToString(), Color.FromArgb(245, 158, 11), x, 10); x += 220;
                AddStatCard("Overdue", stats.OverdueBooks.ToString(), Color.FromArgb(239, 68, 68), x, 10);

                Label lblRecent = new Label();
                lblRecent.Text = "Recent Transactions";
                lblRecent.Font = new Font("Segoe UI", 13, FontStyle.Bold);
                lblRecent.ForeColor = TextDark;
                lblRecent.Location = new Point(20, 135);
                lblRecent.AutoSize = true;
                panelContent.Controls.Add(lblRecent);

                DataGridView dgv = CreateGrid();
                dgv.Location = new Point(20, 165);
                dgv.Size = new Size(panelContent.ClientSize.Width - 40, 280);
                dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                panelContent.Controls.Add(dgv);

                List<BorrowRecord> records = new BorrowRepository().GetAll();
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Book");
                dt.Columns.Add("Member");
                dt.Columns.Add("Student No");
                dt.Columns.Add("Borrow Date");
                dt.Columns.Add("Due Date");
                dt.Columns.Add("Return Date");
                dt.Columns.Add("Status");

                int count = 0;
                foreach (BorrowRecord r in records)
                {
                    if (count >= 15) break;
                    string returnDate = r.ReturnDate.HasValue ? r.ReturnDate.Value.ToString("dd.MM.yyyy") : "-";
                    string status = r.Status == "Borrowed" ? (r.IsOverdue ? "OVERDUE" : "Borrowed") : "Returned";
                    dt.Rows.Add(r.RecordID, r.BookTitle, r.MemberName, r.StudentNumber,
                        r.BorrowDate.ToString("dd.MM.yyyy"), r.DueDate.ToString("dd.MM.yyyy"), returnDate, status);
                    count++;
                }
                dgv.DataSource = dt;
                if (dgv.Columns.Contains("ID")) dgv.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                Label lblErr = new Label();
                lblErr.Text = "Cannot connect to database:\n" + ex.Message;
                lblErr.Font = new Font("Segoe UI", 11);
                lblErr.ForeColor = Color.FromArgb(239, 68, 68);
                lblErr.Location = new Point(20, 10);
                lblErr.AutoSize = true;
                panelContent.Controls.Add(lblErr);
            }
        }

        private void AddStatCard(string title, string value, Color color, int x, int y)
        {
            Panel card = new Panel();
            card.Size = new Size(200, 105);
            card.Location = new Point(x, y);
            card.BackColor = CardBg;
            Panel bar = new Panel();
            bar.Size = new Size(4, 105);
            bar.BackColor = color;
            card.Controls.Add(bar);
            Label lblT = new Label();
            lblT.Text = title; lblT.Font = new Font("Segoe UI", 10);
            lblT.ForeColor = TextMuted; lblT.Location = new Point(18, 12); lblT.AutoSize = true;
            card.Controls.Add(lblT);
            Label lblV = new Label();
            lblV.Text = value; lblV.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            lblV.ForeColor = color; lblV.Location = new Point(18, 38); lblV.AutoSize = true;
            card.Controls.Add(lblV);
            panelContent.Controls.Add(card);
        }

        // ==================== BOOKS ====================
        private void LoadBooksPage()
        {
            panelContent.Controls.Clear();

            ToolStrip ts = new ToolStrip();
            ts.GripStyle = ToolStripGripStyle.Hidden;
            ts.BackColor = Color.FromArgb(226, 232, 240);
            ts.Padding = new Padding(10, 5, 10, 5);

            ToolStripTextBox txtSearch = new ToolStripTextBox();
            txtSearch.Size = new Size(250, 28);
            txtSearch.Font = new Font("Segoe UI", 11);
            txtSearch.TextChanged += delegate { RefreshBooks(txtSearch.Text.Trim()); };
            ts.Items.Add(txtSearch);
            ts.Items.Add(new ToolStripSeparator());

            ToolStripButton btnAdd = new ToolStripButton("+ New Book");
            btnAdd.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAdd.ForeColor = Color.FromArgb(37, 99, 235);
            btnAdd.Click += delegate { ShowBookForm(null); };
            ts.Items.Add(btnAdd);
            ts.Items.Add(new ToolStripSeparator());

            ToolStripButton btnDel = new ToolStripButton("Delete");
            btnDel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnDel.ForeColor = Color.FromArgb(239, 68, 68);
            btnDel.Click += delegate { DeleteSelectedBook(); };
            ts.Items.Add(btnDel);

            panelContent.Controls.Add(ts);

            dgvBooks = CreateGrid();
            dgvBooks.Location = new Point(20, 45);
            dgvBooks.Size = new Size(panelContent.ClientSize.Width - 40, panelContent.ClientSize.Height - 55);
            dgvBooks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvBooks.CellDoubleClick += delegate (object s, DataGridViewCellEventArgs ev)
            {
                if (ev.RowIndex < 0) return;
                int id = Convert.ToInt32(dgvBooks.Rows[ev.RowIndex].Cells["ID"].Value);
                Book book = new BookRepository().GetById(id);
                if (book != null) ShowBookForm(book);
            };
            panelContent.Controls.Add(dgvBooks);

            RefreshBooks("");
        }

        private void DeleteSelectedBook()
        {
            if (dgvBooks == null || dgvBooks.CurrentRow == null)
            {
                MessageBox.Show("Please select a book to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = Convert.ToInt32(dgvBooks.CurrentRow.Cells["ID"].Value);
            string title = dgvBooks.CurrentRow.Cells["Title"].Value.ToString();
            if (MessageBox.Show("Are you sure you want to delete \"" + title + "\"?", "Delete Book",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                new BookRepository().Delete(id);
                MessageBox.Show("Book deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshBooks("");
            }
        }

        private void RefreshBooks(string search)
        {
            List<Book> books = string.IsNullOrEmpty(search)
                ? new BookRepository().GetAll()
                : new BookRepository().Search(search);
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Title"); dt.Columns.Add("Author"); dt.Columns.Add("ISBN");
            dt.Columns.Add("Category"); dt.Columns.Add("Year", typeof(int));
            dt.Columns.Add("Shelf"); dt.Columns.Add("Total", typeof(int));
            dt.Columns.Add("Available", typeof(int));
            foreach (Book b in books)
                dt.Rows.Add(b.BookID, b.Title, b.Author, b.ISBN, b.CategoryName,
                    b.PublishYear, b.ShelfLocation, b.TotalCopies, b.AvailableCopies);
            dgvBooks.DataSource = dt;
            if (dgvBooks.Columns.Contains("ID")) dgvBooks.Columns["ID"].Visible = false;
        }

        private void ShowBookForm(Book existing)
        {
            Form frm = new Form();
            frm.Text = existing == null ? "Add New Book" : "Edit Book";
            frm.Size = new Size(500, 580);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.FormBorderStyle = FormBorderStyle.FixedDialog;
            frm.MaximizeBox = false;
            frm.BackColor = Color.FromArgb(248, 250, 252);

            string[] labels = { "ISBN", "Title", "Author", "Publisher", "Publish Year", "Page Count", "Shelf Location", "Total Copies" };
            TextBox[] txts = new TextBox[labels.Length];
            int yy = 15;
            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label();
                lbl.Text = labels[i]; lbl.Location = new Point(25, yy);
                lbl.Font = new Font("Segoe UI", 9); lbl.AutoSize = true;
                frm.Controls.Add(lbl);
                txts[i] = new TextBox();
                txts[i].Location = new Point(25, yy + 18);
                txts[i].Size = new Size(430, 26);
                txts[i].Font = new Font("Segoe UI", 10);
                frm.Controls.Add(txts[i]);
                yy += 50;
            }

            Label lblCat = new Label();
            lblCat.Text = "Category"; lblCat.Location = new Point(25, yy);
            lblCat.Font = new Font("Segoe UI", 9); lblCat.AutoSize = true;
            frm.Controls.Add(lblCat);
            ComboBox cmbCat = new ComboBox();
            cmbCat.Location = new Point(25, yy + 18); cmbCat.Size = new Size(430, 26);
            cmbCat.Font = new Font("Segoe UI", 10);
            cmbCat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCat.DataSource = new CategoryRepository().GetAll();
            cmbCat.DisplayMember = "CategoryName"; cmbCat.ValueMember = "CategoryID";
            frm.Controls.Add(cmbCat);

            if (existing != null)
            {
                txts[0].Text = existing.ISBN; txts[1].Text = existing.Title;
                txts[2].Text = existing.Author; txts[3].Text = existing.Publisher;
                txts[4].Text = existing.PublishYear.ToString();
                txts[5].Text = existing.PageCount.ToString();
                txts[6].Text = existing.ShelfLocation;
                txts[7].Text = existing.TotalCopies.ToString();
                cmbCat.SelectedValue = existing.CategoryID;
            }

            yy += 55;
            Button btnSave = new Button();
            btnSave.Text = existing == null ? "Save" : "Update";
            btnSave.Size = new Size(200, 36); btnSave.Location = new Point(25, yy);
            btnSave.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnSave.BackColor = AccentBlue; btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat; btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Cursor = Cursors.Hand;
            frm.Controls.Add(btnSave);

            if (existing != null)
            {
                Button btnFormDel = new Button();
                btnFormDel.Text = "Delete"; btnFormDel.Size = new Size(100, 36);
                btnFormDel.Location = new Point(240, yy);
                btnFormDel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                btnFormDel.BackColor = Color.FromArgb(239, 68, 68);
                btnFormDel.ForeColor = Color.White;
                btnFormDel.FlatStyle = FlatStyle.Flat; btnFormDel.FlatAppearance.BorderSize = 0;
                btnFormDel.Click += delegate
                {
                    if (MessageBox.Show("Are you sure you want to delete this book?", "Delete",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        new BookRepository().Delete(existing.BookID);
                        frm.Close();
                        RefreshBooks("");
                    }
                };
                frm.Controls.Add(btnFormDel);
            }

            btnSave.Click += delegate
            {
                if (string.IsNullOrWhiteSpace(txts[1].Text) || string.IsNullOrWhiteSpace(txts[2].Text))
                {
                    MessageBox.Show("Title and Author are required!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Book book = existing != null ? existing : new Book();
                book.ISBN = txts[0].Text.Trim(); book.Title = txts[1].Text.Trim();
                book.Author = txts[2].Text.Trim(); book.Publisher = txts[3].Text.Trim();
                int py; int.TryParse(txts[4].Text, out py); book.PublishYear = py;
                int pc; int.TryParse(txts[5].Text, out pc); book.PageCount = pc;
                book.ShelfLocation = txts[6].Text.Trim();
                int tc; int.TryParse(txts[7].Text, out tc); book.TotalCopies = tc > 0 ? tc : 1;
                if (cmbCat.SelectedValue != null) book.CategoryID = (int)cmbCat.SelectedValue;

                BookRepository repo = new BookRepository();
                if (existing == null)
                {
                    book.AvailableCopies = book.TotalCopies;
                    repo.Add(book);
                    MessageBox.Show("Book added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    repo.Update(book);
                    MessageBox.Show("Book updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                frm.Close();
                RefreshBooks("");
            };
            frm.ShowDialog();
        }

        // ==================== MEMBERS ====================
        private void LoadMembersPage()
        {
            panelContent.Controls.Clear();

            // -- Toolbar panel at top --
            Panel toolbar = new Panel();
            toolbar.Size = new Size(panelContent.ClientSize.Width, 50);
            toolbar.Location = new Point(0, 0);
            toolbar.BackColor = ContentBg;
            toolbar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            TextBox txtSearch = new TextBox();
            txtSearch.Size = new Size(280, 30);
            txtSearch.Location = new Point(20, 10);
            txtSearch.Font = new Font("Segoe UI", 11);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.BackColor = Color.White;
            txtSearch.TextChanged += delegate { RefreshMembers(txtSearch.Text.Trim()); };
            toolbar.Controls.Add(txtSearch);

            Button btnAdd = new Button();
            btnAdd.Text = "+ New Member";
            btnAdd.Size = new Size(140, 32);
            btnAdd.Location = new Point(320, 9);
            btnAdd.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAdd.BackColor = Color.FromArgb(16, 185, 129);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Click += delegate { ShowMemberForm(null); };
            toolbar.Controls.Add(btnAdd);

            Button btnDel = new Button();
            btnDel.Text = "Delete";
            btnDel.Size = new Size(90, 32);
            btnDel.Location = new Point(470, 9);
            btnDel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnDel.BackColor = Color.FromArgb(239, 68, 68);
            btnDel.ForeColor = Color.White;
            btnDel.FlatStyle = FlatStyle.Flat;
            btnDel.FlatAppearance.BorderSize = 0;
            btnDel.Cursor = Cursors.Hand;
            btnDel.Click += delegate { DeleteSelectedMember(); };
            toolbar.Controls.Add(btnDel);

            panelContent.Controls.Add(toolbar);

            // -- Grid below toolbar --
            dgvMembers = CreateGrid();
            dgvMembers.Location = new Point(20, 55);
            dgvMembers.Size = new Size(panelContent.ClientSize.Width - 40, panelContent.ClientSize.Height - 70);
            dgvMembers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMembers.CellDoubleClick += delegate (object s, DataGridViewCellEventArgs ev)
            {
                if (ev.RowIndex < 0) return;
                int id = Convert.ToInt32(dgvMembers.Rows[ev.RowIndex].Cells["ID"].Value);
                Member m = new MemberRepository().GetById(id);
                if (m != null) ShowMemberForm(m);
            };
            panelContent.Controls.Add(dgvMembers);

            // Bring toolbar to front
            toolbar.BringToFront();

            RefreshMembers("");
        }

        private void DeleteSelectedMember()
        {
            if (dgvMembers == null || dgvMembers.CurrentRow == null)
            {
                MessageBox.Show("Please select a member to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = Convert.ToInt32(dgvMembers.CurrentRow.Cells["ID"].Value);
            string name = dgvMembers.CurrentRow.Cells["First Name"].Value.ToString() + " " +
                          dgvMembers.CurrentRow.Cells["Last Name"].Value.ToString();
            if (MessageBox.Show("Are you sure you want to delete \"" + name + "\"?", "Delete Member",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                new MemberRepository().Delete(id);
                MessageBox.Show("Member deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshMembers("");
            }
        }

        private void RefreshMembers(string search)
        {
            List<Member> members = string.IsNullOrEmpty(search)
                ? new MemberRepository().GetAll()
                : new MemberRepository().Search(search);
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Student No"); dt.Columns.Add("First Name");
            dt.Columns.Add("Last Name"); dt.Columns.Add("Email");
            dt.Columns.Add("Phone"); dt.Columns.Add("Department");
            dt.Columns.Add("Max Books", typeof(int));
            foreach (Member m in members)
                dt.Rows.Add(m.MemberID, m.StudentNumber, m.FirstName, m.LastName,
                    m.Email, m.Phone, m.Department, m.MaxBooks);
            dgvMembers.DataSource = dt;
            if (dgvMembers.Columns.Contains("ID")) dgvMembers.Columns["ID"].Visible = false;
        }

        private void ShowMemberForm(Member existing)
        {
            Form frm = new Form();
            frm.Text = existing == null ? "Add New Member" : "Edit Member";
            frm.Size = new Size(480, 500);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.FormBorderStyle = FormBorderStyle.FixedDialog;
            frm.MaximizeBox = false;
            frm.BackColor = Color.FromArgb(248, 250, 252);

            string[] labels = { "Student No", "First Name", "Last Name", "Email", "Phone", "Department", "Max Books" };
            TextBox[] txts = new TextBox[labels.Length];
            int yy = 15;
            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label();
                lbl.Text = labels[i]; lbl.Location = new Point(25, yy);
                lbl.Font = new Font("Segoe UI", 9); lbl.AutoSize = true;
                frm.Controls.Add(lbl);
                txts[i] = new TextBox();
                txts[i].Location = new Point(25, yy + 18);
                txts[i].Size = new Size(410, 26);
                txts[i].Font = new Font("Segoe UI", 10);
                frm.Controls.Add(txts[i]);
                yy += 50;
            }

            if (existing != null)
            {
                txts[0].Text = existing.StudentNumber; txts[1].Text = existing.FirstName;
                txts[2].Text = existing.LastName; txts[3].Text = existing.Email;
                txts[4].Text = existing.Phone; txts[5].Text = existing.Department;
                txts[6].Text = existing.MaxBooks.ToString();
            }
            else { txts[6].Text = "3"; }

            yy += 10;
            Button btnSave = new Button();
            btnSave.Text = existing == null ? "Save" : "Update";
            btnSave.Size = new Size(200, 36); btnSave.Location = new Point(25, yy);
            btnSave.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnSave.BackColor = Color.FromArgb(16, 185, 129);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat; btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Cursor = Cursors.Hand;
            frm.Controls.Add(btnSave);

            if (existing != null)
            {
                Button btnFormDel = new Button();
                btnFormDel.Text = "Delete"; btnFormDel.Size = new Size(100, 36);
                btnFormDel.Location = new Point(240, yy);
                btnFormDel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                btnFormDel.BackColor = Color.FromArgb(239, 68, 68);
                btnFormDel.ForeColor = Color.White;
                btnFormDel.FlatStyle = FlatStyle.Flat; btnFormDel.FlatAppearance.BorderSize = 0;
                btnFormDel.Click += delegate
                {
                    if (MessageBox.Show("Are you sure you want to delete this member?", "Delete",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        new MemberRepository().Delete(existing.MemberID);
                        frm.Close();
                        RefreshMembers("");
                    }
                };
                frm.Controls.Add(btnFormDel);
            }

            btnSave.Click += delegate
            {
                if (string.IsNullOrWhiteSpace(txts[1].Text) || string.IsNullOrWhiteSpace(txts[2].Text))
                {
                    MessageBox.Show("First Name and Last Name are required!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Member m = existing != null ? existing : new Member();
                m.StudentNumber = txts[0].Text.Trim(); m.FirstName = txts[1].Text.Trim();
                m.LastName = txts[2].Text.Trim(); m.Email = txts[3].Text.Trim();
                m.Phone = txts[4].Text.Trim(); m.Department = txts[5].Text.Trim();
                int mx; int.TryParse(txts[6].Text, out mx); m.MaxBooks = mx > 0 ? mx : 3;
                m.ExpiryDate = DateTime.Now.AddYears(1);

                MemberRepository repo = new MemberRepository();
                if (existing == null) { repo.Add(m); MessageBox.Show("Member added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else { repo.Update(m); MessageBox.Show("Member updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                frm.Close();
                RefreshMembers("");
            };
            frm.ShowDialog();
        }

        // ==================== BORROW BOOK ====================
        private void LoadBorrowPage()
        {
            panelContent.Controls.Clear();
            Label lblTitle = new Label();
            lblTitle.Text = "Borrow a Book";
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.ForeColor = TextDark; lblTitle.Location = new Point(20, 10); lblTitle.AutoSize = true;
            panelContent.Controls.Add(lblTitle);

            int yy = 55;
            AddLabel("Select Member:", 20, yy); yy += 22;
            ComboBox cmbMember = new ComboBox();
            cmbMember.Location = new Point(20, yy); cmbMember.Size = new Size(400, 28);
            cmbMember.Font = new Font("Segoe UI", 10);
            cmbMember.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMember.DataSource = new MemberRepository().GetAll();
            cmbMember.DisplayMember = "FullName";
            panelContent.Controls.Add(cmbMember); yy += 42;

            AddLabel("Select Book (available):", 20, yy); yy += 22;
            ComboBox cmbBook = new ComboBox();
            cmbBook.Location = new Point(20, yy); cmbBook.Size = new Size(400, 28);
            cmbBook.Font = new Font("Segoe UI", 10);
            cmbBook.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBook.DataSource = new BookRepository().GetAvailableBooks();
            cmbBook.DisplayMember = "Title";
            panelContent.Controls.Add(cmbBook); yy += 42;

            AddLabel("Due Date:", 20, yy); yy += 22;
            DateTimePicker dtpDue = new DateTimePicker();
            dtpDue.Location = new Point(20, yy); dtpDue.Size = new Size(250, 28);
            dtpDue.Font = new Font("Segoe UI", 10);
            dtpDue.Value = DateTime.Now.AddDays(14); dtpDue.MinDate = DateTime.Now.AddDays(1);
            panelContent.Controls.Add(dtpDue); yy += 48;

            Button btnBorrow = new Button();
            btnBorrow.Text = "Borrow Book"; btnBorrow.Size = new Size(200, 42);
            btnBorrow.Location = new Point(20, yy);
            btnBorrow.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnBorrow.BackColor = AccentBlue; btnBorrow.ForeColor = Color.White;
            btnBorrow.FlatStyle = FlatStyle.Flat; btnBorrow.FlatAppearance.BorderSize = 0;
            btnBorrow.Cursor = Cursors.Hand;
            btnBorrow.Click += delegate
            {
                Member selMember = cmbMember.SelectedItem as Member;
                Book selBook = cmbBook.SelectedItem as Book;
                if (selMember == null || selBook == null)
                { MessageBox.Show("Please select a member and a book!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                try
                {
                    new BorrowRepository().BorrowBook(selBook.BookID, selMember.MemberID,
                        dtpDue.Value, SessionManager.CurrentUser.UserID);
                    MessageBox.Show("Book borrowed successfully!\n\nBook: " + selBook.Title +
                        "\nMember: " + selMember.FullName + "\nDue: " + dtpDue.Value.ToString("dd.MM.yyyy"),
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBorrowPage();
                }
                catch (Exception ex)
                { MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };
            panelContent.Controls.Add(btnBorrow);
        }

        // ==================== RETURN BOOK ====================
        private void LoadReturnPage()
        {
            panelContent.Controls.Clear();
            Label lblTitle = new Label();
            lblTitle.Text = "Return Book - double click to return";
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.ForeColor = TextDark; lblTitle.Location = new Point(20, 10); lblTitle.AutoSize = true;
            panelContent.Controls.Add(lblTitle);

            DataGridView dgv = CreateGrid();
            dgv.Location = new Point(20, 50);
            dgv.Size = new Size(panelContent.ClientSize.Width - 40, panelContent.ClientSize.Height - 65);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelContent.Controls.Add(dgv);

            List<BorrowRecord> active = new BorrowRepository().GetActive();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int)); dt.Columns.Add("Book");
            dt.Columns.Add("Member"); dt.Columns.Add("Student No");
            dt.Columns.Add("Borrow Date"); dt.Columns.Add("Due Date");
            dt.Columns.Add("Days Left", typeof(int)); dt.Columns.Add("Status");
            foreach (BorrowRecord r in active)
                dt.Rows.Add(r.RecordID, r.BookTitle, r.MemberName, r.StudentNumber,
                    r.BorrowDate.ToString("dd.MM.yyyy"), r.DueDate.ToString("dd.MM.yyyy"),
                    r.DaysRemaining, r.IsOverdue ? "OVERDUE" : "Borrowed");
            dgv.DataSource = dt;
            if (dgv.Columns.Contains("ID")) dgv.Columns["ID"].Visible = false;

            dgv.CellDoubleClick += delegate (object s, DataGridViewCellEventArgs ev)
            {
                if (ev.RowIndex < 0) return;
                int recordId = Convert.ToInt32(dgv.Rows[ev.RowIndex].Cells["ID"].Value);
                string bookName = dgv.Rows[ev.RowIndex].Cells["Book"].Value.ToString();
                if (MessageBox.Show("Return \"" + bookName + "\"?", "Return Book",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        new BorrowRepository().ReturnBook(recordId);
                        MessageBox.Show("Book returned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadReturnPage();
                    }
                    catch (Exception ex)
                    { MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            };
        }

        // ==================== REPORTS ====================
        private void LoadReportsPage()
        {
            panelContent.Controls.Clear();
            AddLabel("Reports", 20, 10).Font = new Font("Segoe UI", 14, FontStyle.Bold);
            int yy = 50;
            AddReportCard("Active Borrows", "All currently borrowed books", AccentBlue, yy,
                delegate { ShowReport("Active Borrows", new BorrowRepository().GetActive()); }); yy += 80;
            AddReportCard("Overdue Books", "Books past their due date", Color.FromArgb(239, 68, 68), yy,
                delegate { ShowReport("Overdue Books", new BorrowRepository().GetOverdue()); }); yy += 80;
            AddReportCard("Unpaid Fines", "Members with outstanding fines", Color.FromArgb(245, 158, 11), yy,
                delegate { ShowFinesReport(); });
        }

        private void AddReportCard(string title, string desc, Color color, int y, EventHandler onClick)
        {
            Panel card = new Panel();
            card.Size = new Size(panelContent.ClientSize.Width - 40, 65);
            card.Location = new Point(20, y); card.BackColor = CardBg;
            card.Cursor = Cursors.Hand;
            card.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Panel bar = new Panel(); bar.Size = new Size(4, 65); bar.BackColor = color;
            card.Controls.Add(bar);
            Label lblT = new Label(); lblT.Text = title;
            lblT.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblT.ForeColor = TextDark; lblT.Location = new Point(18, 8); lblT.AutoSize = true;
            card.Controls.Add(lblT);
            Label lblD = new Label(); lblD.Text = desc;
            lblD.Font = new Font("Segoe UI", 9);
            lblD.ForeColor = TextMuted; lblD.Location = new Point(18, 34); lblD.AutoSize = true;
            card.Controls.Add(lblD);
            card.Click += onClick; lblT.Click += onClick; lblD.Click += onClick;
            panelContent.Controls.Add(card);
        }

        private void ShowReport(string title, List<BorrowRecord> records)
        {
            Form frm = new Form(); frm.Text = title; frm.Size = new Size(900, 500);
            frm.StartPosition = FormStartPosition.CenterParent;
            DataGridView dgv = CreateGrid(); dgv.Dock = DockStyle.Fill; frm.Controls.Add(dgv);
            DataTable dt = new DataTable();
            dt.Columns.Add("Book"); dt.Columns.Add("Author"); dt.Columns.Add("Member");
            dt.Columns.Add("Student No"); dt.Columns.Add("Borrow Date");
            dt.Columns.Add("Due Date"); dt.Columns.Add("Days Left", typeof(int)); dt.Columns.Add("Status");
            foreach (BorrowRecord r in records)
                dt.Rows.Add(r.BookTitle, r.BookAuthor, r.MemberName, r.StudentNumber,
                    r.BorrowDate.ToString("dd.MM.yyyy"), r.DueDate.ToString("dd.MM.yyyy"),
                    r.DaysRemaining, r.IsOverdue ? "OVERDUE" : "Borrowed");
            dgv.DataSource = dt; frm.ShowDialog();
        }

        private void ShowFinesReport()
        {
            Form frm = new Form(); frm.Text = "Unpaid Fines"; frm.Size = new Size(800, 450);
            frm.StartPosition = FormStartPosition.CenterParent;
            DataGridView dgv = CreateGrid(); dgv.Dock = DockStyle.Fill; frm.Controls.Add(dgv);
            List<FineRecord> fines = new FineRepository().GetUnpaid();
            DataTable dt = new DataTable();
            dt.Columns.Add("Member"); dt.Columns.Add("Book"); dt.Columns.Add("Amount");
            dt.Columns.Add("Reason"); dt.Columns.Add("Date");
            foreach (FineRecord f in fines)
                dt.Rows.Add(f.MemberName, f.BookTitle, f.Amount.ToString("C"), f.Reason, f.FineDate.ToString("dd.MM.yyyy"));
            dgv.DataSource = dt; frm.ShowDialog();
        }

        // ==================== SETTINGS ====================
        private void LoadSettingsPage()
        {
            panelContent.Controls.Clear();
            AddLabel("System Settings", 20, 10).Font = new Font("Segoe UI", 14, FontStyle.Bold);
            AddLabel("Database Connection String:", 20, 55);
            TextBox txtConn = new TextBox();
            txtConn.Text = DatabaseHelper.ConnectionString;
            txtConn.Location = new Point(20, 78); txtConn.Size = new Size(580, 26);
            txtConn.Font = new Font("Consolas", 9);
            panelContent.Controls.Add(txtConn);

            Button btnTest = new Button();
            btnTest.Text = "Test Connection"; btnTest.Size = new Size(150, 33);
            btnTest.Location = new Point(615, 76);
            btnTest.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnTest.BackColor = Color.FromArgb(16, 185, 129);
            btnTest.ForeColor = Color.White; btnTest.FlatStyle = FlatStyle.Flat;
            btnTest.FlatAppearance.BorderSize = 0; btnTest.Cursor = Cursors.Hand;
            btnTest.Click += delegate
            {
                DatabaseHelper.ConnectionString = txtConn.Text;
                string err; bool ok = DatabaseHelper.TestConnection(out err);
                MessageBox.Show(ok ? "Connection successful!" : "Connection error:\n" + err,
                    "Test", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            };
            panelContent.Controls.Add(btnTest);

            Label lblAbout = new Label();
            lblAbout.Text = "Smart Library v1.0\nLibrary Management System\nC# Windows Forms + SQL Server\nCourse Project";
            lblAbout.Font = new Font("Segoe UI", 10); lblAbout.ForeColor = TextMuted;
            lblAbout.Location = new Point(20, 130); lblAbout.AutoSize = true;
            panelContent.Controls.Add(lblAbout);
        }

        // ==================== HELPERS ====================
        private DataGridView CreateGrid()
        {
            DataGridView dgv = new DataGridView();
            dgv.ReadOnly = true; dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false; dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None; dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(226, 232, 240);
            dgv.Font = new Font("Segoe UI", 9);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgv.DefaultCellStyle.SelectionForeColor = TextDark;
            dgv.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
            dgv.RowTemplate.Height = 34;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 85, 105);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 38;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            return dgv;
        }

        private Label AddLabel(string text, int x, int y)
        {
            Label lbl = new Label(); lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 10); lbl.ForeColor = TextDark;
            lbl.Location = new Point(x, y); lbl.AutoSize = true;
            panelContent.Controls.Add(lbl);
            return lbl;
        }
    }
}
