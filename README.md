# 🎂 Cake Delivery API v2

A clean, secure, and scalable ASP.NET Core Web API for managing cake delivery operations. This project adopts **MongoDB** instead of SQL Server, integrates **Entity Framework Core**, and follows a **Clean Architecture** approach. Features include JWT-based authentication/authorization, FluentValidation, role/permission-based access, and Swagger for documentation.
  
  <img src="https://imgur.com/h1cjR7b.jpg" alt="App Screenshot 1"/>
---

## 📦 Table of Contents

- [🧠 Project Summary](#-project-summary)
- [🛠️ Tech Stack](#-tech-stack)
- [🧱 Project Structure](#-project-structure)
- [🔐 Authentication & Authorization](#-authentication--authorization)
- [📄 Swagger API Documentation](#-swagger-api-documentation)
- [📘 MongoDB + EF Integration](#-mongodb--ef-integration)
- [✅ Validation](#-validation)
- [🧪 Password Hashing](#-password-hashing)
- [🔍 Pagination, Ordering, Searching](#-pagination-ordering-searching)

---

## 🧠 Project Summary

Cake Delivery API v2 is a RESTful API backend for managing all aspects of a cake delivery service, including:

- CRUD operations for cakes, categories, orders, and deliveries
- Authentication using JWT tokens
- Role and permission-based authorization
- Fluent validation for input data
- API documentation with Swagger
- MongoDB as a flexible NoSQL data store

---

## 🛠️ Tech Stack

| Layer            | Tech                                   |
|------------------|----------------------------------------|
| Language         | C#                                     |
| Framework        | ASP.NET Core Web API                   |
| Database         | MongoDB                                |
| ORM              | Entity Framework Core with MongoDriver|
| Auth             | JWT Bearer Token                       |
| Docs             | Swagger (Swashbuckle)                  |
| Validation       | FluentValidation                       |
| Hashing          | BCrypt.Net                             |
| Architecture     | Clean Architecture                     |

---

## 🧱 Project Structure

```bash
CakeDelivery.API/
├── DataAccess/
│   ├── AppDbContext.cs         # Mongo-compatible EF DbContext
│   └── MongoDriverSetup.cs
├── Business/
│   ├── Services/               # Core business logic
│   └── Interfaces/
├── Models/
│   ├── Entities/
│   └── DTOs/
├── Auth/
│   ├── JwtService.cs
│   └── Permission Enum
├── Validation/
│   └── CakeValidator.cs
├── Mapping/
│   └── AutoMapperProfiles.cs
└── Program.cs
```

---

## 🔐 Authentication & Authorization

### ✅ JWT-Based Authentication

JWT tokens are used for secure access. After logging in, a token is issued and must be included in the Authorization header.

### ✅ Role & Permission-Based Authorization

A custom `Permissions` enum allows fine-grained access control.

```csharp
public enum Permissions
{
    View = 1,
    ManageCakes = 2,
    ManageUsers = 4,
    ManageOrders = 8,
    ...
}
```

Each role is granted one or more permissions.

---

## 📄 Swagger API Documentation

Packages used:

```xml
<PackageReference Include="Swashbuckle.AspNetCore" Version="7.3.1" />
<PackageReference Include="Swashbuckle.AspNetCore.Annotations" Version="7.3.1" />
```

This enables interactive API documentation and testing.

---

## 📘 MongoDB + EF Core-Like Integration

- In this project, I used MongoDB, but with a twist. Instead of using the traditional MongoDriver directly, I chose to structure my code similar to Entity Framework Core (EF Core) for better clarity and familiarity.

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Cake> Cakes { get; set; }
    ...
}
```

> 🛑 Note: I know this isn't the standard way to use MongoDB — in real-world scenarios, you’d typically work directly with the Mongo driver. But I wanted to experiment with an EF-style approach to organize my collections in a way that feels clean and intuitive, especially during development.

> 💡 Why I did this:
Coming from an EF Core background, this helped me keep things organized while exploring MongoDB. It also made the transition smoother and kept my codebase consistent across different data providers.

---

## ✅ Validation

Implemented with FluentValidation:

```csharp
RuleFor(cake => cake.CakeName)
    .NotEmpty().WithMessage("Cake name is required.");
```

Validations ensure that API data is clean, consistent, and meaningful.

---

## 🧪 Password Hashing

Implemented using BCrypt:

```csharp
return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
```

Why BCrypt?
- Built-in salt
- Adjustable work factor
- Time-safe verification

---

## 🔍 Pagination, Ordering, Searching

### 📄 Endpoint: GET `/api/Cake`

Retrieve a paginated, ordered list of cakes.

**Query Parameters:**

| Param       | Type     | Required | Description                                      |
|-------------|----------|----------|--------------------------------------------------|
| PageNumber  | int      | ✅        | Page number (starting from 1)                   |
| PageSize    | int      | ✅        | Number of items per page                        |
| OrderBy     | string   | ❌        | Field name to sort by (e.g., CakeName, Price)   |
| Ascending   | bool     | ❌        | true = ASC, false = DESC                        |

```http
GET /api/Cake?pageNumber=1&pageSize=10&orderBy=CreatedAt&ascending=false
```
|  |  |
|------------|----------|
| ![input](https://imgur.com/mwtuXM2.jpg) | ![output](https://imgur.com/hrraY3u.jpg) |
| ![input](https://imgur.com/uQh5dMm.jpg) | ![output](https://imgur.com/lBQmqYL.jpg) |
---

### 🔍 Endpoint: POST `/api/Cake/search`

Search cakes by a specific field and value.

**Request Body:**
```json
{
  "field": "CakeName",
  "value": "Chocolate"
}
```

Supports fields like:
- `CakeName`
- `CakeId`

  
 |  |  |
|------------|----------|
| ![input](https://imgur.com/lR9QHzE.jpg) | ![output](https://imgur.com/U6zhS59.jpg) |
