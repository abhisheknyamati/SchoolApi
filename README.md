# SchoolAPI

A structured Web API for managing school operations, with a focus on clean architecture and testability.

## Project Structure

- **API Project**: Contains controller-specific components (DTOs, validators, mappers).
- **Business Layer (Class Library)**: Contains repositories, DbContext, entity models, and services.

## Features Implemented

- **Database Setup**
  - Entity Framework Core
  - SQL Server Database Connection
  - Database Migrations

- **Repository Pattern**
  - Centralized data access via repositories
  - Unit of Work Pattern for transaction management

- **Student Controller**
  - CRUD operations
  - Pagination and Search functionality
  - Age Calculation Service

- **Unit Testing**
  - Controller Tests using Moq for repository mocking
  - Repository Tests using Microsoft.EntityFrameworkCore.InMemory

- **API Testing**
  - Comprehensive endpoint testing

## Getting Started

### Prerequisites

- .NET Core SDK
- SQL Server
- Moq Library
- Microsoft.EntityFrameworkCore.InMemory

### Running the Application

1. Clone the repository
2. Configure database connection in `appsettings.json`
3. Run database migrations
4. Start the API project

---

## License

Distributed under the MIT License.
