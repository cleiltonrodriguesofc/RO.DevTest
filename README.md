# Rota das Oficinas Technical Test - Project Documentation

## Introduction
This repository is a .NET 8.0 Web API implementing a basic e-commerce system for managing **Customers**, **Products**, and **Sales**. It builds upon the provided template, preserving the original architecture (CQRS, Repository Pattern) and integrating with PostgreSQL via Entity Framework Core.

## Implemented Features
So far, the following functionality has been completed and validated:

### 1. Customer Management (CRUD)
- **Create**: `POST /api/customer` with `CustomerCreateRequest`.  
- **Read**:
  - `GET /api/customer/{id}` returns single customer details.  
  - `GET /api/customer` returns list of all customers.  
- **Update**: `PUT /api/customer/{id}` with `CustomerUpdateRequest`.  
- **Delete**: `DELETE /api/customer/{id}`.
- **Unit Tests** covering all success and failure scenarios.

### 2. Product Management (CRUD)
- **Create**: `POST /api/product` with `ProductCreateRequest`.  
- **Read**:
  - `GET /api/product/{id}` returns product by ID.  
  - `GET /api/product` returns all products.  
- **Update**: `PUT /api/product/{id}` with `ProductUpdateRequest`.  
- **Delete**: `DELETE /api/product/{id}`.
- **Unit Tests** covering valid update, non-existent product, and other edge cases.

### 3. Sale Management (CRUD)
- **Create**: `POST /api/sale` with `SaleCreateRequest`, debits product stock, calculates total.  
- **Read**:
  - `GET /api/sale/{id}` returns sale details including `saleId`, `customerName`, and each item's `productCode`, `productName`, `price`, `quantity`.  
  - `GET /api/sale` returns all sales with the same projection.  
- **Update**: `PUT /api/sale/{id}` with `SaleUpdateRequest`, supports adjusting items, stock, and total.  
- **Delete**: `DELETE /api/sale/{id}` removes a sale if it exists.  
- **Unit Tests** for update and delete handlers, covering success and failure paths.

## Technical Details
- **Framework**: .NET 8.0 Web API
- **ORM**: Entity Framework Core
- **Database**: PostgreSQL
- **Architecture**: CQRS + Repository Pattern
- **Dependency Injection**: `Program.cs` registers all handlers and repositories
- **Serialization**: `ReferenceHandler.Preserve` to handle cycles
- **Testing**: xUnit, Moq, FluentAssertions for unit tests
- **Version Control**: GitFlow branching model, semantic commits (`feat(...)`, `fix(...)`)

## Project Structure
```
/RO.DevTest.Domain/         # Entities
/RO.DevTest.Application/    # DTOs, Interfaces, UseCases
/RO.DevTest.Persistence/    # EF Core Configurations, Migrations
/RO.DevTest.WebApi/         # Controllers, Program.cs
/RO.DevTest.Tests/          # Unit tests per use case
```

## Remaining Requirements
The following features from the technical test are still pending:

1. **User Authentication & Authorization**  
   - Login endpoint, JWT token generation, role-based access control.

2. **Pagination, Filtering, Sorting**  
   - Add query parameters (`page`, `pageSize`, `sort`, `filter`) to list endpoints.

3. **Sales Analytics Endpoints**  
   - Given a date range, return:
     - Total number of sales
     - Total revenue
     - Revenue per product sold

4. **Integration Tests**  
   - Test API endpoints end-to-end using `WebApplicationFactory` or similar.

5. **Docker Support**  
   - `Dockerfile` and `docker-compose.yml` for the API and PostgreSQL.

6. **Frontend Interface**  
   - Optional: basic UI for customer, product, sale management and analytics.

## How to Run
1. **Database**: ensure a PostgreSQL instance is running and update `appsettings.json` connection string.  
2. **Migrations**: `dotnet ef database update` in the Persistence project.  
3. **Run API**: `dotnet run` in the WebApi project root.  
4. **Swagger UI**: navigate to `https://localhost:{port}/swagger`.

---

*This documentation will be updated as new features are implemented.*

