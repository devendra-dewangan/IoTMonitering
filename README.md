# IoT Monitoring System

A comprehensive IoT device monitoring platform built with **ASP.NET Core 7**, **Angular**, and **SQL Server**. The system supports multiple communication protocols for device telemetry ingestion, real-time data streaming, and secure user/device authentication.

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────────────┐
│                        IoT Monitoring System                        │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │                    Frontend (Angular)                        │   │
│  │  ┌──────────┐  ┌──────────┐  ┌─────────────────────────┐   │   │
│  │  │   Auth   │  │Dashboard │  │   Real-time Updates     │   │   │
│  │  │ (Login/  │  │  (Home)  │  │   (SignalR Client)      │   │   │
│  │  │ Register)│  │          │  │                         │   │   │
│  │  └──────────┘  └──────────┘  └─────────────────────────┘   │   │
│  └──────────────────────┬──────────────────────────────────────┘   │
│                         │ HTTP / WebSocket                         │
│  ┌──────────────────────▼──────────────────────────────────────┐   │
│  │                   Backend (ASP.NET Core 7)                   │   │
│  │                                                              │   │
│  │  ┌────────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐  │   │
│  │  │ REST API   │  │ SignalR  │  │  gRPC    │  │ TCP/UDP  │  │   │
│  │  │ Controllers│  │   Hub    │  │ Service  │  │ Services │  │   │
│  │  └────────────┘  └──────────┘  └──────────┘  └──────────┘  │   │
│  │                                                              │   │
│  │  ┌──────────────────────────────────────────────────────┐   │   │
│  │  │              Business Logic Layer                    │   │   │
│  │  │  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────┐ │   │   │
│  │  │  │  Auth    │  │  Device  │  │Telemetry │  │Token │ │   │   │
│  │  │  │ Service  │  │  Service │  │ Service  │  │Service│ │   │   │
│  │  │  └──────────┘  └──────────┘  └──────────┘  └──────┘ │   │   │
│  │  └──────────────────────────────────────────────────────┘   │   │
│  │                                                              │   │
│  │  ┌──────────────────────────────────────────────────────┐   │   │
│  │  │              Data Access Layer (EF Core)              │   │   │
│  │  │  ┌──────────┐  ┌──────────┐  ┌──────────────────┐   │   │   │
│  │  │  │ AppDbContext│ │Unit of  │  │  Repositories   │   │   │   │
│  │  │  │           │  │  Work   │  │                  │   │   │   │
│  │  │  └──────────┘  └──────────┘  └──────────────────┘   │   │   │
│  │  └──────────────────────────────────────────────────────┘   │   │
│  └──────────────────────┬──────────────────────────────────────┘   │
│                         │                                          │
│  ┌──────────────────────▼──────────────────────────────────────┐   │
│  │                    SQL Server Database                        │   │
│  │  ┌──────────┐  ┌──────────┐  ┌──────────────────────────┐   │   │
│  │  │  Users   │  │ Devices  │  │       Telemetry          │   │   │
│  │  │(Identity)│  │          │  │  (Temperature, Humidity) │   │   │
│  │  └──────────┘  └──────────┘  └──────────────────────────┘   │   │
│  └─────────────────────────────────────────────────────────────┘   │
│                                                                     │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │              Mock Devices (Simulator)                        │   │
│  │  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐   │   │
│  │  │ TCP      │  │ UDP      │  │ REST     │  │ gRPC     │   │   │
│  │  │ Client   │  │ Client   │  │ Client   │  │ Client   │   │   │
│  │  └──────────┘  └──────────┘  └──────────┘  └──────────┘   │   │
│  │  ┌──────────────────────────────────────────────────────┐   │   │
│  │  │              SignalR Hub Client                      │   │   │
│  │  └──────────────────────────────────────────────────────┘   │   │
│  └─────────────────────────────────────────────────────────────┘   │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Project Structure

```
IoTMonitering/
├── IotMonitering.App/                    # ASP.NET Core Backend API
│   ├── Config/                           # Configuration models
│   ├── Controllers/                      # REST API controllers
│   │   ├── AuthController.cs             # User authentication endpoints
│   │   ├── DeviceAuthController.cs       # Device authentication endpoints
│   │   ├── DeviceController.cs           # Device management endpoints
│   │   └── TelemetryController.cs        # Telemetry data endpoints
│   ├── Data/                             # EF Core DbContext & migrations
│   ├── DTOs/                             # Data Transfer Objects
│   ├── Grpc/                             # gRPC service implementations
│   │   └── TelemetryGrpcService.cs
│   ├── Hubs/                             # SignalR hubs
│   │   └── TelemetryHub.cs
│   ├── Repository/                       # Repository & Unit of Work pattern
│   ├── Services/                         # Business logic services
│   │   ├── AuthService.cs
│   │   ├── DeviceService.cs
│   │   ├── TelemetryService.cs
│   │   └── TokenService.cs
│   ├── TCP/                              # TCP server for device ingestion
│   │   └── TcpServerService.cs
│   ├── UDP/                              # UDP server for device ingestion
│   │   └── UdpServerService.cs
│   ├── Program.cs                        # Application entry point
│   ├── appsettings.json                  # Application configuration
│   └── dockerfile                        # Docker build file
│
├── IoTMonitering.Domain/                 # Domain layer
│   ├── Entity/                           # Domain entities
│   │   ├── Device.cs                     # IoT device entity
│   │   ├── Telemetry.cs                  # Telemetry data entity
│   │   └── User.cs                       # Identity user entity
│   └── Protos/                           # gRPC protobuf definitions
│       └── telemetry.proto
│
├── IoTMonitering.MockDevices/            # Device simulator
│   ├── Clients/                          # Communication protocol clients
│   │   ├── IClient.cs                    # Client interface
│   │   ├── TelemetryGrpcClient.cs        # gRPC client
│   │   ├── TelemetryRestClient.cs        # REST API client
│   │   ├── TelemetryTcpClient.cs         # TCP client
│   │   ├── TelemetryUdpClient.cs         # UDP client
│   │   └── TelemtryHubClient.cs          # SignalR hub client
│   ├── Configurations/                   # Simulator configuration
│   │   ├── DeviceInfo.cs
│   │   └── ServerInfo.cs
│   ├── HostedServices/                   # Background device simulation
│   │   └── DeviceWoker.cs
│   └── Program.cs                        # Simulator entry point
│
├── IoTMonitering.Ui/                     # Angular Frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/                     # Core module (auth, guards, services)
│   │   │   ├── features/                 # Feature modules
│   │   │   │   ├── auth/                 # Login & Register pages
│   │   │   │   └── dashboard/            # Dashboard with home view
│   │   │   └── shared/                   # Shared components
│   │   ├── environments/                 # Environment configs
│   │   └── styles.css                    # Global styles (Tailwind)
│   ├── angular.json
│   ├── tailwind.config.js
│   └── package.json
│
├── dokcer/sql/                           # SQL initialization scripts
├── docker-compose.yml                    # Docker Compose configuration
├── .drone.yml                            # CI/CD pipeline (Drone CI)
└── IoTMonitoring.sln                     # .NET Solution file
```

## Features

### Backend (ASP.NET Core 7)

- **Multi-Protocol Device Ingestion**
  - **TCP Server** - Raw TCP socket server for device data ingestion
  - **UDP Server** - UDP socket server for lightweight device data ingestion
  - **REST API** - HTTP endpoints for device telemetry submission
  - **gRPC** - High-performance gRPC service for telemetry streaming
  - **SignalR** - Real-time WebSocket hub for live telemetry updates

- **Authentication & Authorization**
  - JWT-based user authentication with refresh tokens
  - Device-level authentication using device keys
  - ASP.NET Core Identity for user management
  - Role-based access control

- **Data Management**
  - SQL Server database with Entity Framework Core
  - Repository pattern with Unit of Work
  - Telemetry data storage (temperature, humidity)
  - Device registration and management

- **Real-time Capabilities**
  - SignalR hub for broadcasting telemetry to connected clients
  - gRPC bidirectional streaming

### Frontend (Angular)

- **User Authentication** - Login and registration pages
- **Dashboard** - Real-time device monitoring dashboard
- **Responsive Design** - Tailwind CSS for modern UI
- **Lazy Loading** - Feature modules for optimized performance

### Mock Device Simulator

- Simulates IoT devices sending telemetry data
- Supports all communication protocols (TCP, UDP, REST, gRPC, SignalR)
- Configurable device count and data generation intervals
- Background hosted service for continuous simulation

## Getting Started

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Node.js 18+](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or Docker)
- [Docker](https://www.docker.com/) (optional, for containerized setup)

### Running with Docker Compose

```bash
# Clone the repository
git clone https://github.com/devendra-dewangan/IoTMonitering.git
cd IoTMonitering

# Start all services
docker-compose up -d
```

The application will be available at:
- **REST API**: `http://localhost:5000`
- **SignalR Hub**: `http://localhost:5001/deviceHub`
- **gRPC Service**: `http://localhost:5002`
- **TCP Server**: `localhost:6000`
- **UDP Server**: `localhost:6001`

### Running Locally

#### 1. Database Setup

Ensure SQL Server is running and update the connection string in `IotMonitering.App/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TestDB;User Id=SA;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
  }
}
```

#### 2. Backend

```bash
# Restore dependencies
dotnet restore

# Apply database migrations
cd IotMonitering.App
dotnet ef database update

# Run the API
dotnet run
```

#### 3. Frontend

```bash
cd IoTMonitering.Ui

# Install dependencies
npm install

# Start development server
ng serve
```

The Angular app will be available at `http://localhost:4200`.

#### 4. Mock Devices

```bash
cd IoTMonitering.MockDevices

# Run the device simulator
dotnet run
```

## Configuration

### Server Configuration (`appsettings.json`)

| Setting | Default | Description |
|---------|---------|-------------|
| `RestApi.Port` | `7000` | REST API HTTP port |
| `SignalR.Port` | `5001` | SignalR WebSocket port |
| `SignalR.Route` | `deviceHub` | SignalR hub endpoint path |
| `Grpc.Port` | `5002` | gRPC service port |
| `Tcp.Port` | `6000` | TCP server port |
| `Udp.Port` | `6001` | UDP server port |

### Docker Port Mapping

| Port | Protocol | Service |
|------|----------|---------|
| `5000` | HTTP | REST API |
| `5001` | HTTP | SignalR Hub |
| `5002` | HTTP/2 | gRPC |
| `6000` | TCP | TCP Device Ingestion |
| `6001` | UDP | UDP Device Ingestion |

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | User login |
| POST | `/api/auth/refresh` | Refresh JWT token |

### Device Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/device` | Get all user devices |
| POST | `/api/device` | Register a new device |
| GET | `/api/device/{id}` | Get device details |
| PUT | `/api/device/{id}` | Update device |
| DELETE | `/api/device/{id}` | Delete device |

### Device Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/device-auth/authenticate` | Authenticate a device |

### Telemetry

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/telemetry` | Submit telemetry data |
| GET | `/api/telemetry/device/{deviceId}` | Get device telemetry history |

## Communication Protocols

### REST API
Standard HTTP/1.1 REST endpoints for device registration and telemetry submission.

### SignalR
Real-time WebSocket communication for live telemetry streaming. Connect to `/deviceHub` endpoint.

### gRPC
High-performance RPC framework for efficient telemetry streaming. Protobuf definitions in `IoTMonitering.Domain/Protos/telemetry.proto`.

### TCP
Raw TCP socket server for devices with limited HTTP capabilities. Data is parsed as structured telemetry.

### UDP
Lightweight UDP socket server for low-bandwidth device communication.

## CI/CD

The project uses **Drone CI** for continuous integration. The pipeline configuration is in `.drone.yml`:

```yaml
steps:
  - name: build
    image: mcr.microsoft.com/dotnet/sdk:7.0
    commands:
      - dotnet restore
      - dotnet build --configuration Release
```

## Tech Stack

| Layer | Technology |
|-------|-----------|
| **Backend** | ASP.NET Core 7, C# |
| **Frontend** | Angular 16+, TypeScript |
| **Database** | SQL Server, Entity Framework Core |
| **Real-time** | SignalR, gRPC |
| **Auth** | JWT, ASP.NET Core Identity |
| **Container** | Docker, Docker Compose |
| **CI/CD** | Drone CI |
| **Styling** | Tailwind CSS |

## License

This project is licensed under the MIT License.