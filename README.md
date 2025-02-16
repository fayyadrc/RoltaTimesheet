# Timesheet Management System

The **Timesheet Management System** is a robust and comprehensive full-stack web application developed to streamline the tracking of project hours and monitoring work progress. It replaces the traditional Google Sheets-based process, offering dedicated interfaces for employees and administrators to enhance user experience and data management efficiency.
###(This was created during an internship)
---

## Features

### Admin Dashboard:
- Manage projects and monitor employee work progress.
- View, edit, and delete timesheet records.
- Generate reports for detailed analysis.

### Employee Dashboard:
- Log project hours easily with an intuitive interface.
- View past entries and track individual work hours.

### Backend Functionality:
- **ASP.NET MVC** and **C#** power the server-side logic for reliable performance.
- **SQL Server** ensures secure and efficient data storage.

---

## Why Use This System?

The Timesheet Management System addresses the inefficiencies of manual time-tracking methods like Google Sheets. It offers:
- Automation of project hour tracking.
- Real-time insights into project progress.
- Improved accuracy and reduced administrative workload.

---

## Usage

### Administrator:
1. Log in to access the admin dashboard.
2. Add, edit, or remove projects.
3. Manage employee hours and generate reports.

### Employee:
1. Log in to access the employee dashboard.
2. Log work hours for assigned projects.
3. View and edit previous entries.

---

## Architecture and Code Overview

The system follows the **MVC (Model-View-Controller)** design pattern for separation of concerns:

### Models:
- Represent database entities and business logic.
- Located in the `Models` folder.

### Views:
- Frontend UI components built with HTML, CSS, and JavaScript.
- Located in the `Views` folder.

### Controllers:
- Handle HTTP requests and responses.
- Coordinate between the model and view.
- Located in the `Controllers` folder.

### SQL Server:
- Stores user data, project details, and timesheet entries.
