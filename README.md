# Lines# Lines - Ride Sharing Platform

<div align="center">
  [![.NET](https://img.shields.io/badge/.NET-6.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
  [![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-6DB33F?logo=architecture)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
  [![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
</div>

## Overview

Lines is a comprehensive ride-sharing platform that connects passengers with drivers, built with modern .NET technologies following Clean Architecture principles. The platform provides a seamless experience for booking rides, managing trips, and handling payments.

## Architecture

The application follows Clean Architecture with clear separation of concerns across multiple layers:

```
Lines/
├── Presentation/        # API Controllers, DTOs, and API-related logic
├── Application/         # Application use cases and business logic
├── Domain/              # Core business models and interfaces
├── Infrastructure/      # External concerns (DB, external services)
└── Admin/               # Admin portal and management features
```

### Key Architectural Patterns

- **Clean Architecture** - Separation of concerns with dependency rule
- **CQRS** - Command Query Responsibility Segregation
- **Repository Pattern** - For data access abstraction
- **Unit of Work** - For transaction management
- **Mediator Pattern** - Using MediatR for in-process messaging

## Features

### Core Features

- **User Management**
  - Passenger and driver registration with multi-step verification
  - Role-based access control (Passenger, Driver, Admin)
  - Authentication & Authorization using JWT

- **Ride Booking**
  - Real-time ride requests and matching
  - Fare estimation
  - Multiple stop support
  - Scheduled rides

- **Driver Features**
  - Vehicle management
  - Document verification
  - Earnings tracking
  - Rating system

- **Payment System**
  - Multiple payment methods
  - Wallet integration
  - Promo codes and discounts
  - Receipt generation

## Technology Stack

- **Backend**: .NET 6+
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT with Identity
- **Real-time**: SignalR
- **Caching**: Distributed caching with Redis
- **Logging**: Serilog
- **Testing**: xUnit, Moq, Shouldly

## Project Structure

- **Lines.Presentation**: API controllers, DTOs, and API-specific logic
- **Lines.Application**: Application services, use cases, and business logic
- **Lines.Domain**: Core domain models, interfaces, and domain events
- **Lines.Infrastructure**: Data access, external services, and infrastructure concerns
- **Admin**: Web-based admin portal for system management

## Getting Started

### Prerequisites

- .NET 6.0 SDK or later
- SQL Server 2019 or later
- Node.js (for frontend development)

### Installation

1. Clone the repository
   ```bash
   git clone https://github.com/your-organization/lines.git
   cd Lines/Source/Lines
   ```

2. Configure the database connection string in `appsettings.json`

3. Run database migrations
   ```bash
   dotnet ef database update --project Lines.Infrastructure --startup-project Lines.Presentation
   ```

4. Run the application
   ```bash
   dotnet run --project Lines.Presentation
   ```

## Documentation

- [API Documentation](docs/api.md)
- [Database Schema](docs/database.md)
- [Deployment Guide](docs/deployment.md)

## Contributing

Contributions are welcome! Please read our [contributing guidelines](CONTRIBUTING.md) to get started.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

For any questions or support, please contact [o.ahmed9847@gmail.com](mailto:o.ahmed9847@gmail.com)

---
<div align="center">
  Made with ❤️ by Lines Team
</div>
