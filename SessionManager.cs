using SmartLibrary.Models;

namespace SmartLibrary
{
    public static class SessionManager
    {
        public static User CurrentUser { get; set; }
        public static bool IsLoggedIn { get { return CurrentUser != null; } }
        public static bool IsAdmin { get { return CurrentUser != null && CurrentUser.Role == "Admin"; } }
    }
}
