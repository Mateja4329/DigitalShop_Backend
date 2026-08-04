# 🛒 DigitalShop API - E-commerce Backend

This repository contains a backend RESTful API for an e-commerce platform, developed during a software internship at **TIAC** in Novi Sad. The project was built using **ASP.NET Core** framework (C#) and **SQLite** database, with the aim of demonstrating modern principles of creating scalable and secure web applications.

The system enables complete management of users, products and shopping carts, with strict access control and advanced reporting based on business logic.

## 🚀 Key technologies and concepts

* **Architecture (N-Tier):** Logical separation into presentation layer (Controllers), business logic (Services) and data access (Repositories), respecting the principles of clean code and *Dependency Injection*.
* **Security (JWT & BCrypt):** JSON Web Token (JWT) implementation for role-based authentication and authorization (*Customer* and *Admin*). Passwords are never stored in clear text, but protected by modern hashing algorithms (*BCrypt*).
* **Database (EF Core & SQLite):** Used *Code-First* approach with Entity Framework Core ORM to efficiently map relations (One-to-Many, Many-to-Many) in a lightweight, fast SQLite database.
* **Validation (FluentValidation):** Robust validation of input data at the level of DTO (Data Transfer Object) classes, before the data even reaches the business logic.
* **Advanced LINQ Queries:** Optimized database queries to generate dynamic analytical reports (eg find best selling products by category).
* **Documentation (Swagger):** Integrated and custom Swagger UI that automatically generates documentation and allows API testing directly from the browser with JWT support.

## 📸 Application Screenshots

### 1. API Documentation & Architecture
[![Swagger UI](docs/CartAPI.png)](DigitalShop.Api/Controllers/CartController.cs)
[![Swagger UI](docs/Product&ReportAPI.png)](DigitalShop.Api/Controllers/ProductController.cs)
[![Swagger UI](docs/UserAPI.png)](DigitalShop.Api/Controllers/UserController.cs)

[![Logged Out State](docs/LoggedOut.png)](DigitalShop.Api/Program.cs)
[![User Registration Request](docs/UserRegister.png)](DigitalShop.Application/Validators/UserValidators/UserRegistratorValidator.cs)
[![User Registration 201 Created](docs/UserRegister201.png)](DigitalShop.Api/Controllers/UserController.cs)
[![User Login Request](docs/UserLogin.png)](DigitalShop.Application/Validators/UserValidators/UserLoginValidator.cs)
[![JWT Token Generated 200 OK](docs/UserLogin200_Token.png)](DigitalShop.Application/Helpers/AuthHelpers.cs)
[![Inserting Bearer Token](docs/TokenAuth.png)](DigitalShop.Api/Program.cs)
[![Token Authorized Status](docs/TokenAuthSuccess.png)](DigitalShop.Api/Program.cs)
[![Logged In & Protected Endpoints Available](docs/LoggedIn.png)](DigitalShop.Api/Controllers/UserController.cs)

### 3. Database State (SQLite)
[![SQLite Users Table](docs/sqlite_user.png)](DigitalShop.Infrastructure/Data/DataContext.cs)
[![SQLite Products Table](docs/sqlite_product.png)](DigitalShop.Infrastructure/Data/DataContext.cs)
[![SQLite Carts Table](docs/sqlite_cart.png)](DigitalShop.Infrastructure/Data/DataContext.cs)

### 4. Advanced Analytics & Reporting (LINQ)
[![Sales Report Analytics](docs/ReportAPI.png)](DigitalShop.Infrastructure/Repo/ReportRepository.cs)
