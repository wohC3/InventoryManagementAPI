# Inventory Management API

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-REST_API-512BD4?style=for-the-badge&logo=dotnet)
![EF Core](https://img.shields.io/badge/Entity_Framework_Core-7C3AED?style=for-the-badge)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite)
![JWT](https://img.shields.io/badge/JWT-Authentication-000000?style=for-the-badge&logo=jsonwebtokens)
![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=for-the-badge&logo=swagger)
![xUnit](https://img.shields.io/badge/xUnit-Tested-5C2D91?style=for-the-badge)

A demo/learning project for building an Inventory Management REST API using **ASP.NET Core**.

---
## Table of Contents

- [Tech Stack](#tech-stack)
- [Features](#features)
- [API Endpoint Summary](#api-endpoint-summary)
- [Authentication](#authentication)
- [Authorization](#authorization)
- [Architecture](#architecture)
- [Testing](#testing)
- [Running the Application](#running-the-application)
- [Running Tests](#running-tests)

---

## Tech Stack
- ASP.NET Core 8
- Entity Framework Core
- SQLite
- BCrypt.Net-Next (Password Hashing)
- JWT Bearer Authentication
- Swagger / OpenAPI
- xUnit
- SQLite InMemory (Service Testing)
- Moq (Controller Testing)

[Back to top](#inventory-management-api)

---

## Features

### Products

- Create a product (Name, Quantity, Price)
- Retrieve all products with pagination
- Retrieve a product by ID
- Update an existing product
- Delete a product
- Product validation

### Users

- Get all users *(Admin only)*
- Update user roles *(Admin only)*

[Back to top](#inventory-management-api)

---

## API Endpoint Summary

| Method | Endpoint | Authorization | Description |
|:------:|----------|:-------------:|-------------|
| **POST** | `/api/auth/register` | Public | Register a new user account |
| **POST** | `/api/auth/login` | Public | Authenticate a user and receive a JWT token |
| **GET** | `/api/products?page={page}&pageSize={pageSize}` | Public | Retrieve a paginated list of products |
| **GET** | `/api/products/{id}` | Public | Retrieve a product by its ID |
| **POST** | `/api/products` | Authenticated | Create a new product |
| **PUT** | `/api/products/{id}` | Authenticated | Update an existing product |
| **DELETE** | `/api/products/{id}` | Admin | Delete a product |
| **GET** | `/api/users` | Admin | Retrieve all users |
| **PUT** | `/api/users/{id}/role` | Admin | Update a user's role |

[Back to top](#inventory-management-api)



---
## Authentication

- Register new users
- Secure password storage using BCrypt
- Login with username and password
- Generate JWT authentication tokens
- Protect API endpoints using JWT Bearer Authentication
- Role-based authorization

[Back to top](#inventory-management-api)

---

## Authorization

### Admin

- View all users
- Update user roles

### User

- Access product endpoints
- Cannot access admin-only endpoints

[Back to top](#inventory-management-api)

---

## Architecture

- Service Layer pattern for business logic
- DTOs to separate API models from database entities
- Data Annotations for request validation
- Entity Framework Core for database access

[Back to top](#inventory-management-api)

---

## Testing

### Service Layer

Using:

- xUnit
- SQLite InMemory
- Entity Framework Core

#### Current Coverage

##### ProductService

- AddProduct
- GetProductById
- GetAllProducts (Pagination)
- UpdateProductById
- DeleteProductById

##### UserService

- GetAllUsers
- UpdateUserRole

##### AuthService

- Register
- Login

[Back to top](#inventory-management-api)

---

### Controller Layer

Using:

- xUnit
- Moq
- ASP.NET Core MVC testing types

Controllers are tested using mocked service interfaces.

#### Current Coverage

##### ProductController

- GetProductById
- GetProducts
- PostProduct
- UpdateProductById
- DeleteProductById

##### UsersController

- GetUsers
- UpdateUserRoleById

##### AuthController

- Register
- Login

[Back to top](#inventory-management-api)

---

## Running the Application

### Requirements

- .NET 8 SDK

### Start the API

```bash
dotnet run
```

Open Swagger:

```text
http://localhost:xxxx/swagger/index.html
```

Replace `xxxx` with the port shown in the terminal.

[Back to top](#inventory-management-api)

---

## Running Tests

```bash
cd API.Tests
dotnet test
```
Or for more details
```bash
dotnet test --logger "console;verbosity=detailed"
```

[Back to top](#inventory-management-api)

---


