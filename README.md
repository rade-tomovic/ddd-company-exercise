# Employee-Company Management API

## Overview

This API service provides handling for companies and with employees. It is built on .NET 6, using Postgres for employee and company data storage, and MongoDB for system logs. It allows management of `Employee`, `Company`, and `SystemLog` entities with specific business rules.

## Features

### Employee Management (`POST /api/employees`)

- **Create Employee**: Create a new Employee by specifying their email, title, and the companies they should be added to.
- **Validation**: Unique employee email and unique title within a company.

### Company Management (`POST /api/companies`)

- **Create Company**: Create a new Company by specifying its name and an array of employees to be created and added.
- **Validation**: Unique company name and same validation rules for employees as the Employee Management feature.

### System Logs

- Keeps track of events such as the creation and updating of Employee and Company resources.

## Getting Started

### Prerequisites

- .NET 6
- MongoDB for System Logs
- Postgres for Employee and Company storage

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/rade-tomovic/ddd-company-exercise.git
   ```

2. Navigate to the project directory and restore the packages:

   ```bash
   dotnet restore
   ```

3. Handling databases:

   Handling database can be done from NuGet package manager console

   Adding migrations:

   ```powershell
   Add-Migration <Migration_Name> -Project CompanyManager.Persistence -StartupProject CompanyManager.Api -Context CompaniesDbContext
   ```

   Updating database:

   ```powershell
   Update-Database -Project CompanyManager.Persistence -StartupProject CompanyManager.Api -Context CompaniesDbContext
   ```

4. Run the API:

   ```bash
   dotnet run
   ```

5. Run tests:

   ```bash
   dotnet test
   ```

## Sample Payloads

### Adding an Employee:

```json
{
  "email": "j.doe@example.com",
  "title": "Developer",
  "companyIds": ["Company1_ID", "Company2_ID"]
}
```

### Adding a Company:

```json
{
  "name": "NewCompany",
  "employees": [
    { "email": "new_employee@gmail.com" },
    { "id": "EXISTING_EMPLOYEE_ID" }
  ]
}
```

## Technology Stack

- **Framework**: ASP.NET Core Web API 6
- **Database for Employees and Companies**: PostgreSQL
- **Database for System Logs**: MongoDB
- **Logging**: Serilog
- **Validation**: Fluent Validation
- **Architecture**: Onion Architecture with CQRS using MediatR
