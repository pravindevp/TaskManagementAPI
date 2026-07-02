# Task Management API

A RESTful Web API built with **ASP.NET Core (.NET 8)** and **SQL Server** for managing tasks. The application includes JWT Authentication, CRUD operations, structured logging, global exception handling, and a background service that automatically expires overdue tasks.

## Features

- User Registration & Login (JWT Authentication)
- CRUD Operations for Tasks
- Filter by Status & Sort by Due Date
- Password Hashing with BCrypt
- Global Exception Handling
- Serilog Logging
- Background Service to Expire Overdue Tasks
- Swagger UI with JWT Authorization

## Tech Stack

- ASP.NET Core (.NET 8)
- Entity Framework Core 8
- SQL Server
- JWT Authentication
- BCrypt
- Serilog
- Swagger

## Project Structure

```text
Controllers/
Services/
Repositories/
Interfaces/
Models/
DTOs/
Data/
Middleware/
BackgroundJobs/
Migrations/
Program.cs
```

## Database Setup

Run the SQL script:

```text
Scripts/Database.sql
```

Update the connection string in `appsettings.json`.

## Run the Application

```bash
dotnet run
```

Open Swagger:

```text
https://localhost:7080/swagger
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register User |
| POST | `/api/auth/login` | Login User |
| GET | `/api/tasks` | Get All Tasks |
| GET | `/api/tasks/{id}` | Get Task By Id |
| POST | `/api/tasks` | Create Task |
| PUT | `/api/tasks/{id}` | Update Task |
| DELETE | `/api/tasks/{id}` | Delete Task |

## Testing

1. Register a user.
2. Login to get a JWT token.
3. Click **Authorize** in Swagger and paste the token.
4. Test the Task APIs.
5. Create a task with a due date 1 minute ahead to verify automatic expiry.

## Assumptions

- Uses `DateTime.Now` for timestamps.
- Task status is stored as an integer:
  - `0 = Pending`
  - `1 = Completed`
  - `2 = Expired`
- All authenticated users can manage all tasks.
- Background service runs every minute to expire overdue tasks.

## Postman

`TaskManagementAPI.postman_collection.json`
