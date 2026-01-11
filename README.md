# MaxFit+ Fitness Management System

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4)](https://docs.microsoft.com/aspnet/core)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927)](https://www.microsoft.com/sql-server)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.0-7952B3)](https://getbootstrap.com/)

A comprehensive fitness center management system built with **ASP.NET Core MVC 9.0**, featuring role-based access control, class scheduling, membership management, and trainer assignments. Includes separate dashboards for **Admin**, **Staff**, and **Members** with modern UI/UX design.

## 🌟 Features

### 👤 Role-Based Access Control
- **Admin Dashboard**: Complete system management and oversight
- **Staff Dashboard**: Class management and member interaction
- **Member Dashboard**: Personal fitness tracking and class bookings

### 🏋️ Class Management
- Create and schedule fitness classes
- Room and trainer assignment
- Capacity management
- Photo attachments for classes
- Real-time availability tracking
- Conflict detection for room and trainer scheduling

### 👥 User Management
- Member registration and profile management
- Staff account creation and management
- Secure authentication system
- Profile photo uploads

### 📊 Dashboard Features
- Personalized statistics for each role
- Upcoming sessions and class schedules
- Quick action buttons for common tasks
- Modern, responsive UI design

### 🎨 UI/UX
- Modern glassmorphism design
- Dark theme with vibrant colors
- Responsive layout for all devices
- Smooth animations and transitions
- Premium visual aesthetic

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core MVC 9.0
- **Language**: C# 13.0
- **Database**: SQL Server with Entity Framework Core
- **Frontend**: 
  - Bootstrap 5.0
  - Font Awesome Icons
  - Custom CSS with modern design patterns
- **Authentication**: ASP.NET Core Identity
- **Architecture**: MVC Pattern with Repository Pattern

## 📋 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, or full version)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/keremsaliherol/MaxFit-.git
cd MaxFit-
```

### 2. Update Database Connection String
Edit `appsettings.json` and update the connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MaxFitDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 3. Apply Database Migrations
```bash
cd Maxfit+/Maxfit+
dotnet ef database update
```

### 4. Run the Application
```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## 📂 Project Structure

```
Maxfit+/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── ClassesController.cs
│   ├── MembersController.cs
│   └── StaffController.cs
├── Models/              # Data models and ViewModels
├── Data/                # Database context and migrations
├── Views/               # Razor views
│   ├── Account/
│   ├── Admin/
│   ├── Classes/
│   ├── Members/
│   ├── Staff/
│   └── Shared/
├── wwwroot/            # Static files
│   ├── css/
│   ├── images/
│   └── uploads/
└── Program.cs          # Application entry point
```

## 🔐 Security Features

- Password hashing with ASP.NET Core Identity
- Role-based authorization
- Secure file upload handling
- CSRF protection
- SQL injection prevention via Entity Framework

## 📸 Screenshots

### Login Page
Modern login interface with clean design

### Admin Dashboard
Comprehensive overview with statistics and management tools

### Member Dashboard
Personalized fitness tracking and class booking interface

## 🎯 Key Functionalities

### For Admins
- Manage all users (Staff & Members)
- Create and manage fitness classes
- View system-wide statistics
- Access control management

### For Staff
- Manage assigned classes
- View member information
- Track attendance
- Schedule management

### For Members
- Browse and book classes
- View personal schedule
- Track fitness progress
- Manage profile information

## 🔄 Future Enhancements

- [ ] Payment integration
- [ ] Attendance tracking system
- [ ] Workout plan management
- [ ] Nutrition planning
- [ ] Mobile app integration
- [ ] Email notifications
- [ ] Advanced reporting system

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Kerem Salih Erol**
- GitHub: [@keremsaliherol](https://github.com/keremsaliherol)

## 🙏 Acknowledgments

- Built with ASP.NET Core MVC
- UI inspired by modern fitness applications
- Icons by Font Awesome
- Design framework by Bootstrap

---

⭐ **Star this repository if you find it helpful!**
