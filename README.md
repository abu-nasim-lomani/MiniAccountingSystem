# Mini Account Management System - Qtec Solution Ltd. Task

This repository contains the solution for the Junior .NET Developer technical assessment from Qtec Solution Limited. It is a fully functional mini accounting system built with ASP.NET Core Razor Pages, following all the specified technical requirements and architecture.

## 🚀 Live Demo & Screenshots

*(IMPORTANT: Take screenshots of your running application and add them here. This is the best way to showcase your work!)*

**1. Hierarchical Chart of Accounts Page**
![Chart of Accounts Screenshot](https://placehold.co/600x400/EEE/31343C?text=Chart+of+Accounts+Screenshot)

**2. Dynamic Voucher Entry Form**
![Voucher Entry Screenshot](https://placehold.co/600x400/EEE/31343C?text=Voucher+Entry+Screenshot)

**3. User Role Management (Admin View)**
![User Management Screenshot](https://placehold.co/600x400/EEE/31343C?text=User+Management+Screenshot)


---

## ✨ Features Implemented

### Core Features
- **Chart of Accounts Management:** Full CRUD (Create, Read, Update, Delete) functionality for accounts.
- **Hierarchical Tree View:** Accounts are displayed in a professional, easy-to-read parent-child tree structure.
- **Dynamic Voucher Entry:** A multi-line form with real-time total calculation for entering Journal, Payment, and Receipt vouchers. The form enforces the rule that total debits must equal total credits.
- **User & Role Management:** An admin-only section to view all registered users and assign them roles (`Admin`, `Accountant`, `Viewer`).
- **Role-Based Authorization:** A robust security system that controls access to pages and UI elements (menus, buttons) based on the logged-in user's role.

### 🏆 Bonus Feature
- **Excel Export:** The Chart of Accounts report can be downloaded as a formatted `.xlsx` file.

---

## 🛠️ Technology Stack & Architecture

This project was built following the specific technical guidelines provided in the task.

- **Backend:** ASP.NET Core 8 with Razor Pages
- **Database:** MS SQL Server
- **Data Access:** **Stored Procedures Only**. No LINQ or standard Entity Framework queries were used for data manipulation, strictly adhering to the task requirements.
- **ORM:** Dapper (for calling stored procedures) and Entity Framework Core (for Identity management only).
- **Authentication & Authorization:** ASP.NET Core Identity with custom role-based policies.
- **UI:** Bootstrap 5, HTML5, and vanilla JavaScript (for dynamic voucher form).
- **Excel Export:** ClosedXML library.

The project follows a clean architecture with a dedicated **Data Access Layer (Repositories)** to separate database logic from the UI logic.

---

## ⚙️ Setup and Installation

To run this project on your local machine, please follow these steps:

1.  **Prerequisites:**
    * .NET 8 SDK
    * Visual Studio 2022
    * MS SQL Server (or SQL Server Express)

2.  **Clone the Repository:**
    ```bash
    git clone [Your-GitHub-Repository-URL]
    ```

3.  **Database Setup:**
    * Open the project in Visual Studio.
    * Open the `appsettings.json` file.
    * Modify the `DefaultConnection` string to point to your local SQL Server instance.
    * Open SQL Server Management Studio (SSMS), create a new empty database with the name you specified in the connection string (e.g., `QtecAccountsDB`).
    * The application will automatically create all the necessary tables (for Identity and the custom tables) the first time it runs.

4.  **Create the First Admin User:**
    * Open the `Program.cs` file.
    * Find the line `var adminEmail = "your-admin-email@example.com";`
    * Change the email address to an email you will use for registration.
    * Run the application (F5).
    * Register a new user with that same email address. The application will automatically assign the "Admin" role to this user upon the next startup.

5.  **Run the Application:**
    * Log in with your new Admin user account. You will now have access to all features.
