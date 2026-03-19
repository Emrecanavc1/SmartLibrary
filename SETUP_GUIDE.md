# Smart Library - Setup Guide

## Prerequisites

**Only Visual Studio 2022 is needed!** (Community Edition is free)
- Download: https://visualstudio.microsoft.com/downloads/
- During installation, select: **.NET desktop development** workload

That's it! The database is created automatically using LocalDB (included with Visual Studio).

## How to Run

1. Open `SmartLibrary.sln` in Visual Studio
2. Press **F5** (or click the green Start button)
3. The database will be created automatically on first run
4. Login: `admin` / `admin123`

## Login Credentials

| Username | Password | Role |
|----------|----------|------|
| admin | admin123 | Admin |
| librarian | lib123 | Librarian |

## Troubleshooting

| Problem | Solution |
|---------|----------|
| Build failed | Make sure ".NET desktop development" workload is installed in VS |
| Database error on startup | LocalDB may need to be started. Open cmd and run: `sqllocaldb start MSSQLLocalDB` |
| "Cannot connect" | Go to Settings page in the app and update the connection string |
