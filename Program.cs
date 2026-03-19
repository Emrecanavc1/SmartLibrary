using System;
using System.Windows.Forms;
using SmartLibrary.DataAccess;
using SmartLibrary.Forms;

namespace SmartLibrary
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Auto-create database if it doesn't exist
            try
            {
                DatabaseHelper.EnsureDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database setup error:\n\n" + ex.Message +
                    "\n\nThe application will start but may not work until the database is configured.",
                    "Database Setup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Application.Run(new LoginForm());
        }
    }
}
