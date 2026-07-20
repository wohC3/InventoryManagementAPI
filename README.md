# InventoryManagementAPI
Demo/learning project for inventory management REST API. 

Tech stack:
-ASP.NET Core
-EF Core
-SQLite
-Bcrypt.Net-Next (password hashing)
-JWT Bearer Authentication
-Swagger/OpenAPI
-XUnit
-SQLite InMemory (testing)
-Moq (testing)

Features:
-Add a product with name, quantity and price.
-Get list of all products in the database.
-Get specific product from database with id.
-Delete a product with id.
-Update existing product.
-Product validation.
-Get list of all users.
-Update user's role by id.

Authentication:
-Register users with username and password.
-Store passwords securely using BCrypt hashing.
-Login users using username and password.
-Generate JWT authentication tokens.
-Protect API endpoints using JWT Bearer authentication.
-Role authorization for endpoints.

Authorization:
-Only Admin can view all users and update user roles.
-Regular User can't access admin-only endpoints.

Architecture:
-Service layer pattern for business logic.
-DTOs used for separating API models from database entities.
-Data Annotations used for request validation.
-Entity Framework Core used for database access.

Testing:
Service-layer unit tests using:
-XUnit
-SQLite InMemory
-Entity Framework Core

Current Coverage:
ProductService:
	-AddProduct
	-GetProductById
	-GetAllProducts (pagination)
	-UpdateProductById
	-DeleteProductById
UserService:
	-GetAllUsers
	-UpdateUserRole
AuthService:
	-Register
	-Login

Controller-layer unit tests using:
-XUnit
-Moq
-ASP.NET Core MVC testing types

Controllers are tested using mocked service interfaces.

Current Coverage:
ProductController:
	-GetProductById
	-GetProducts
	-PostProduct
	-UpdateProductById
	-DeleteProductById

Run the tests:
in bash:
cd API.Tests
dotnet test

How to run:
-Use .NET version 8
-in bash dotnet run
-go to http://localhost:xxxx/swagger/index.html
