
=======
# 📚 Smart Library — Library Management System ===       v 1.0

A desktop application for university library operations built with **C# Windows Forms** and **SQL Server**.

>>>>>>>
![.NET Framework](https://img.shields.io/badge/.NET_Framework-4.8-512BD4?style=flat&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat&logo=microsoftsqlserver&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual_Studio-5C2D91?style=flat&logo=visualstudio&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-yellow.svg)

---

<<<<<<< HEAD
## 📋 Version History

### v1.01 (Current)
- Added sidebar admin actions (Add Book, Add Member buttons)
- Fixed panel content rendering issue
- Added delete functionality for books and members
- Auto-database creation using LocalDB (zero manual setup)
- English language UI throughout

### v1.0
- Initial release
- Core library management features
- SQL Server database with stored procedures
=======
## 🖥️ Screenshots

| Login Screen | Dashboard |
|:---:|:---:|
| Dark themed login with role-based auth | Real-time stats + recent transactions |

| Books Management | Borrow & Return |
|:---:|:---:|
| CRUD + live search + category filter | Stock control + auto fine calculation |

> *Replace these descriptions with actual screenshots after running the app.*
>>>>>>> 2bda7a38ca3ccc8c8a05ad1aa9fe0cebe9c364de

---

## ✨ Features

- **User Authentication** — Secure login with Admin and Librarian roles
- **Dashboard** — Real-time statistics: total books, members, active borrows, overdue count
- **Book Management** — Full CRUD operations with live search by title, author, or ISBN
- **Member Management** — Student registration with department tracking and book limits
- **Borrow System** — Stock availability check, member limit validation, configurable due dates
- **Return System** — Double-click to return, automatic late fine calculation ($2/day)
- **Reports** — Active borrows, overdue books, unpaid fines
<<<<<<< HEAD
- **Sidebar Admin Actions** — Quick access Add Book and Add Member buttons in sidebar
=======
>>>>>>> 2bda7a38ca3ccc8c8a05ad1aa9fe0cebe9c364de
- **Settings** — Connection string configuration with live test
- **Auto Database** — Database creates itself on first run (zero manual SQL setup)

---

## 🏗️ Architecture

```
SmartLibrary/
├── Program.cs                    # Entry point + auto DB creation
├── SessionManager.cs             # Global user session state
│
<<<<<<< HEAD
├── Models/                       # Data classes
│   ├── Book.cs                   # Book entity (32 lines)
│   ├── Member.cs                 # Member entity (29 lines)
│   ├── BorrowRecord.cs           # Borrow/return tracking (30 lines)
│   └── OtherModels.cs            # User, Category, DashboardStats, FineRecord (51 lines)
│
├── DataAccess/                   # Database operations
│   ├── DatabaseHelper.cs         # Connection manager + auto-create DB (392 lines)
│   ├── BookRepository.cs         # Book CRUD operations (166 lines)
│   ├── MemberRepository.cs       # Member CRUD operations (131 lines)
│   ├── BorrowRepository.cs       # Borrow/return + stored procedure calls (147 lines)
│   └── OtherRepositories.cs      # User, Category, Fine repositories (97 lines)
│
├── Forms/                        # UI layer
│   ├── LoginForm.cs              # Authentication screen (145 lines)
│   └── MainForm.cs               # Main app with all pages (929 lines)
│
├── Database/
│   └── SmartLibraryDB.sql        # Full database script
=======
├── Models/                       # Data classes (142 lines)
│   ├── Book.cs                   # Book entity
│   ├── Member.cs                 # Member entity
│   ├── BorrowRecord.cs           # Borrow/return tracking
│   └── OtherModels.cs            # User, Category, DashboardStats, FineRecord
│
├── DataAccess/                   # Database operations (933 lines)
│   ├── DatabaseHelper.cs         # Connection manager + auto-create DB
│   ├── BookRepository.cs         # Book CRUD operations
│   ├── MemberRepository.cs       # Member CRUD operations
│   ├── BorrowRepository.cs       # Borrow/return + stored procedure calls
│   └── OtherRepositories.cs      # User, Category, Fine repositories
│
├── Forms/                        # UI layer (1,119 lines)
│   ├── LoginForm.cs              # Authentication screen
│   └── MainForm.cs               # Main app (Dashboard, Books, Members, etc.)
│
├── Database/
│   └── SmartLibraryDB.sql        # Full database script (tables + SPs + seed data)
>>>>>>> 2bda7a38ca3ccc8c8a05ad1aa9fe0cebe9c364de
│
├── SmartLibrary.csproj           # Project file (.NET Framework 4.8)
├── SmartLibrary.sln              # Solution file
└── SETUP_GUIDE.md                # Installation instructions
```

**3-Tier Architecture:**

| Layer | Technology | Responsibility |
|-------|-----------|----------------|
<<<<<<< HEAD
| Presentation | Windows Forms | UI components, event handling, sidebar navigation |
| Data Access | ADO.NET | SQL queries, parameterized commands, stored procedure calls |
| Database | SQL Server / LocalDB | 6 tables, 4 indexes, 3 stored procedures |
=======
| Presentation | Windows Forms | UI components, event handling |
| Data Access | ADO.NET | SQL queries, stored procedure calls |
| Database | SQL Server / LocalDB | Tables, indexes, stored procedures |
>>>>>>> 2bda7a38ca3ccc8c8a05ad1aa9fe0cebe9c364de

---

## 🗄️ Database Schema

| Table | Purpose | Key Columns |
|-------|---------|-------------|
| **Users** | System authentication | Username, PasswordHash, Role |
| **Categories** | Book categorization | CategoryName, Description |
| **Books** | Book catalog + inventory | ISBN, Title, Author, AvailableCopies |
| **Members** | Library members | StudentNumber, Name, Department, MaxBooks |
| **BorrowRecords** | Borrow/return tracking | BookID, MemberID, DueDate, Status |
| **Fines** | Late fee management | Amount, Reason, IsPaid |

**Stored Procedures:**
- `sp_BorrowBook` — Validates availability + member limit, creates record, updates stock (transactional)
- `sp_ReturnBook` — Processes return, restores stock, auto-calculates late fine ($2/day)
- `sp_GetDashboardStats` — Aggregates real-time statistics for dashboard

---

## 🚀 Quick Start

### Prerequisites

- **Visual Studio 2022** with **.NET desktop development** workload

That's it! The database is created automatically using LocalDB.

### Run

1. Clone the repository:
   ```bash
   git clone https://github.com/Emrecanavc1/SmartLibrary.git
   ```
2. Open `SmartLibrary.sln` in Visual Studio
3. Press **F5** to build and run
4. Database is created automatically on first launch

### Login Credentials

| Username | Password | Role |
|----------|----------|------|
| `admin` | `admin123` | Admin |
| `librarian` | `lib123` | Librarian |

---

## 🔧 Configuration

The default connection string uses **LocalDB** (included with Visual Studio):

```
Server=(localdb)\MSSQLLocalDB;Database=SmartLibraryDB;Trusted_Connection=True;
```

<<<<<<< HEAD
To use a different SQL Server instance, update via **Settings** page in the app:
=======
To use a different SQL Server instance, update via **Settings** page in the app or edit `DataAccess/DatabaseHelper.cs`:
>>>>>>> 2bda7a38ca3ccc8c8a05ad1aa9fe0cebe9c364de

| SQL Server Type | Connection String |
|-----------------|-------------------|
| LocalDB (default) | `Server=(localdb)\MSSQLLocalDB;Database=SmartLibraryDB;Trusted_Connection=True;` |
| SQL Server Express | `Server=.\SQLEXPRESS;Database=SmartLibraryDB;Trusted_Connection=True;` |
| Default Instance | `Server=.;Database=SmartLibraryDB;Trusted_Connection=True;` |

---

## 🔒 Security & Design Patterns

| Pattern | Implementation |
|---------|---------------|
| **Repository Pattern** | Separate repository class per entity |
| **Parameterized Queries** | `cmd.Parameters.AddWithValue()` prevents SQL injection |
| **Soft Delete** | `IsActive` flag preserves historical records |
| **Transaction Management** | `BEGIN TRANSACTION / COMMIT / ROLLBACK` in stored procedures |
| **Using Statement** | Auto-disposal of `SqlConnection` prevents connection leaks |
| **Session Management** | Static `SessionManager` class for global user state |

---

## 📊 Project Stats

| Metric | Value |
|--------|-------|
<<<<<<< HEAD
| Total Lines of Code | 2,236+ |
=======
| Total Lines of Code | 2,236 |
>>>>>>> 2bda7a38ca3ccc8c8a05ad1aa9fe0cebe9c364de
| Source Files | 13 |
| Database Tables | 6 |
| Stored Procedures | 3 |
| Application Screens | 7 |
| Report Types | 3 |

---

## 🛣️ Roadmap

<<<<<<< HEAD
- [x] Book CRUD with search
- [x] Member CRUD with search
- [x] Sidebar admin quick actions
- [x] Auto-database creation (LocalDB)
- [x] Borrow and return with fine calculation
- [ ] Password hashing (SHA-256)
- [ ] Login attempt limiting
- [ ] Book reservation / hold system
- [ ] Email notifications (due date reminders)
=======
- [ ] Book reservation / hold system
- [ ] Email notifications (due date reminders, overdue alerts)
>>>>>>> 2bda7a38ca3ccc8c8a05ad1aa9fe0cebe9c364de
- [ ] Dashboard charts (pie charts, bar graphs)
- [ ] Export reports to Excel / PDF
- [ ] ISBN barcode scanner
- [ ] Member self-service portal
- [ ] Multi-language support (EN/TR)
- [ ] Dark mode / theme switcher
<<<<<<< HEAD
- [ ] Audit logging
=======
>>>>>>> 2bda7a38ca3ccc8c8a05ad1aa9fe0cebe9c364de

---

## 🛠️ Built With

- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) — Programming language
- [.NET Framework 4.8](https://dotnet.microsoft.com/) — Runtime
- [Windows Forms](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/) — UI framework
- [SQL Server](https://www.microsoft.com/en-us/sql-server/) — Database
- [ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/) — Data access
- [Visual Studio 2022](https://visualstudio.microsoft.com/) — IDE

---

## 📝 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/NewFeature`)
3. Commit your changes (`git commit -m 'Add NewFeature'`)
4. Push to the branch (`git push origin feature/NewFeature`)
5. Open a Pull Request

---

<<<<<<< HEAD
> **Smart Library v1.01** — University Course Project © 2025
=======
> **Smart Library** — University Course Project © 2026
>>>>>>> 2bda7a38ca3ccc8c8a05ad1aa9fe0cebe9c364de
