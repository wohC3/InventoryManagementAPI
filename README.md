# InventoryManagementAPI
Demo/learning project for inventory management REST API. 

Tech stack:
-ASP.NET Core
-EF Core
-Sqlite
-Bcrypt.Net-Next (password hashing)
-JWT Bearer Authentication
-Swagger/OpenAPI

Features:
-Add a product with name, quantity and price.
-Get list of all products in the database.
-Get specific product from database with id.
-Delete a product with id.
-Update existing product.
-Product validation.

Authentication:
-Register users with username and password.
-Store passwords securely using BCrypt hashing.
-Login users using username and password.
-Generate JWT authentication tokens.
-Protect API endpoints using JWT Bearer authentication.

How to run:
-Use .NET version 8
-in bash dotnet run
-go to http://localhost:xxxx/swagger/index.html
