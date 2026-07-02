# Task Management API

A RESTful Web API for managing tasks, built with ASP.NET Core (.NET 8) and SQL Server.
It supports full CRUD, JWT authentication, structured logging, global exception
handling, and a background job that automatically expires overdue tasks.

## Features

- CRUD endpoints for tasks (create, read, update, delete)
- Optional filtering by status and sorting by due date
- JWT bearer authentication (register / login)
- Passwords hashed with BCrypt
- Serilog logging to console and rolling daily files (`Logs/`)
- Global exception-handling middleware with consistent JSON error responses
- Background job (runs every minute) that marks overdue `Pending` tasks as `Expired`
- Swagger UI with a built-in "Authorize" button for testing

## Tech Stack

- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core 8
- Microsoft SQL Server
- JWT Bearer Authentication
- Serilog
- Swagger / OpenAPI

## Project Structure

```
TaskManagementAPI/
├── Controllers/        # HTTP endpoints (Tasks, Auth)
├── Services/           # Business logic (TaskService, AuthService, TokenService)
├── Repositories/       # Data access (TaskRepository)
├── Interfaces/         # Abstractions
├── Models/             # Entities + status enum
├── DTOs/               # Request/response models
├── Data/               # ApplicationDbContext
├── Middleware/         # Exception handling + custom exceptions
├── BackgroundJobs/     # TaskExpiryService
├── Migrations/         # EF Core migrations
└── Program.cs          # Startup & DI configuration
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (Express or Developer edition)
- EF Core CLI tool: `dotnet tool install --global dotnet-ef`

## Configuration

Update `appsettings.json`:

1. **Connection string** – point `DefaultConnection` at your SQL Server instance.
   - Windows auth: `Server=localhost\SQLEXPRESS;Database=TaskManagement;Trusted_Connection=True;TrustServerCertificate=True;`
   - SQL auth: `Server=localhost;Database=TaskManagement;User Id=sa;Password=YourPassword;TrustServerCertificate=True;`
2. **JWT key** – replace `Jwt:Key` with a long random secret (at least 32 characters).

> The committed `appsettings.json` contains placeholders only, no real credentials.

## Database Setup

**Option A – EF Core migrations (recommended):**

```bash
cd TaskManagementAPI
dotnet ef database update
```

**Option B – SQL script:**

Run `Scripts/Database.sql` in SQL Server Management Studio or Azure Data Studio.

## Run

```bash
cd TaskManagementAPI
dotnet run
```

Then open the Swagger UI (the URL is printed in the console, e.g. `https://localhost:7080/swagger`).

## How to Test

1. `POST /api/auth/register` with `{ "username": "admin", "password": "secret123" }`
2. `POST /api/auth/login` with the same credentials → copy the returned `token`
3. Click **Authorize** in Swagger and paste the token
4. Use the `/api/tasks` endpoints
5. To see the background job: create a task with a `dueDate` about a minute
   in the future, wait, then GET it — its status changes to `Expired`

A Postman collection is included: `TaskManagementAPI.postman_collection.json`.

## API Endpoints

| Method | Route                | Auth | Description                          |
|--------|----------------------|------|--------------------------------------|
| POST   | `/api/auth/register` | No   | Register a new user                  |
| POST   | `/api/auth/login`    | No   | Authenticate and receive a JWT       |
| GET    | `/api/tasks`         | Yes  | Get all tasks (filter/sort optional) |
| GET    | `/api/tasks/{id}`    | Yes  | Get a task by id                     |
| POST   | `/api/tasks`         | Yes  | Create a task                        |
| PUT    | `/api/tasks/{id}`    | Yes  | Update a task                        |
| DELETE | `/api/tasks/{id}`    | Yes  | Delete a task                        |

### Example: create a task

```json
POST /api/tasks
{
  "title": "Finish the assessment",
  "description": "Submit before the deadline",
  "dueDate": "2026-07-10T18:00:00Z"
}
```

## Business Rules

- `Title` is required.
- `DueDate` cannot be in the past when creating a task.
- New tasks default to `Pending`.
- Valid statuses: `Pending`, `Completed`, `Expired`.

## Assumptions

- Timestamps are stored in UTC.
- The status enum is persisted as a readable string (e.g. `"Pending"`).
- One authenticated user can see and manage all tasks (tasks are not
  scoped per user, matching the scope of the assessment).
