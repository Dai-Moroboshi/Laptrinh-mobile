# Web API Project (Exam 05)

## Overview
This is a .NET Core Web API for a Restaurant Management System. It includes features for Customer management, Menu management, Reservation management, and Tables management.

## Technologies
- .NET 8/9/10 (ASP.NET Core Web API)
- Entity Framework Core
- SQLite (Configured for portability) / SQL Server (Supported)
- JWT Authentication

## Setup & Running
1. **Prerequisites**: .NET SDK installed.
2. **Database**: The project is configured to use SQLite (`app.db`).
   - Run `dotnet ef database update` to apply migrations (already done).
   - Databse seeding happens automatically on startup.
3. **Run**:
   ```bash
   dotnet run
   ```
4. **Swagger UI**: Access `http://localhost:5000/swagger` or `https://localhost:5001/swagger` (port may vary, check console output) to explore the API.

## API Endpoints
### Authentication
- `POST /api/auth/register`: Register new customer.
- `POST /api/auth/login`: Login (Returns JWT and `student_id: "p5"`).

### Customers
- `GET /api/customers`: List all (Admin).
- `GET /api/customers/{id}`: Get details.
- `GET /api/customers/{id}/reservations`: Get customer reservations.

### Menu Items
- `GET /api/menu-items`: List with filters (search, category, etc.).
- `POST /api/menu-items`: Create item (Admin).
- `PUT /api/menu-items/{id}`: Update item (Admin).
- `DELETE /api/menu-items/{id}`: Delete item (Admin).

### Reservations
- `POST /api/reservations`: Create reservation.
- `POST /api/reservations/{id}/items`: Add items to reservation.
- `PUT /api/reservations/{id}/confirm`: Confirm reservation & assign table (Admin).
- `POST /api/reservations/{id}/pay`: Pay logic.

### Tables
- `GET /api/tables`: List tables.
- `POST /api/tables`: Create table (Admin).

## Notes
- Database file `app.db` will be created in the project root.
- Seed data includes Admin (`admin@example.com` / `adminpassword`) and sample customers/menu items.
