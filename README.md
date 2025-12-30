# IT-ROOTS-TASK

A web application for managing student course registrations, built with **ASP.NET Core 10.0**, **Dapper**, and **MS SQL Server**.


## Features
### For Students
- **Authentication**: Secure Login, Registration, and Password Reset.
- **Email Verification**: Mandatory email verification before login.
- **Course Management**: 
  - Browse available courses.
  - Register for courses.
  - View "My Courses".
  - Unregister (Time-restricted based on semester start).
- **Localization**: Full English and Arabic support (RTL).

### For Administrators
- **Course Management**: CRUD operations (Create, Read, Update, Delete) for courses.
- **Validation**: Prevents deletion of courses with active registrations.

## Architecture
- **Pattern**: 3-Tier Architecture (Web, Core, Data).
- **ORM**: Dapper for high-performance data access.
- **Database**: MS SQL Server.
- **Security**: BCrypt hashing, Token-based verification.
- **Design**: Bootstrap 5 with responsive layout and sticky footer.

## Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or Developer)

## Database Setup
The database scripts are located in `ITRoots-task/Database/Scripts/`. Run them in the following order:

1.  **01_CreateDatabase.sql**: Creates the `StudentRegistrationSystem` database.
2.  **02_CreateTables.sql**: Creates tables (`Users`, `Courses`, `Registrations`) and constraints.
3.  **03_CreateAdminUser.sql**: Inserts a default admin user.
    *   *Default Admin*: `admin` / `Admin@123`
4.  **05_SeedData.sql**: (Optional) Populates the database with sample courses.

**Connection String**:
Update `appsettings.json` in `StudentRegistrationSystem.Web` if your instance name differs:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StudentRegistrationSystem;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

## Installation & Running

1.  **Clone/Download** the repository.
2.  **Navigate** to the solution root.
3.  **Restore Dependencies**:
    ```bash
    dotnet restore
    ```
4.  **Build**:
    ```bash
    dotnet build
    ```
5.  **Run**:
    ```bash
    cd StudentRegistrationSystem.Web
    dotnet run
    ```
6.  OPEN browser to `https://localhost:7029` (or the port shown in console).

## Usage Guide

### Logging In
- **Admin**: Use `admin`/`Admin@123`. Access the dashboard via the "Admin" menu item.
- **Student**: Click "Register" to create a new account. Check your email (simulated in logs/console) for the verification link.

### Localization
- Click "العربية" or "English" in the navigation bar to switch languages.


