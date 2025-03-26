# Car Auction Management System – Microservices Version

This repository contains the **Car Auction Management System** using **microservices**. It has two main APIs (**Auctions API** and **Vehicles API**) that communicate using **MassTransit**. For more details about the first version, see the [Car Auction Management System README](https://github.com/vagnerjsmello/car-auction-management-system).



---

## Features

You can:

1. **Register vehicles** (Sedan, SUV, Hatchback, Truck), each with a unique ID.
2. **Search vehicles** by type, maker, model or year.
3. **Start and end auctions** for vehicles, making sure only one auction is active per vehicle.
4. **Place bids** in active auctions, making sure each new bid is higher.
5. **Handle errors** with clear error messages using **FluentValidation** and a **Notification Pattern**.

---

## Solution Structure

The project is organised like this:

```
CarAuctionManagement.sln
└── src
    ├── AuctionsApi
    │   ├── CAMS.Auctions.Api
    │   ├── CAMS.Auctions.Application
    │   ├── CAMS.Auctions.Data
    │   ├── CAMS.Auctions.Domain
    │   └── CAMS.Auctions.Tests
    ├── VehiclesApi
    │   ├── CAMS.Vehicles.Api
    │   ├── CAMS.Vehicles.Application
    │   ├── CAMS.Vehicles.Data
    │   ├── CAMS.Vehicles.Domain
    │   └── CAMS.Vehicles.Tests
    ├── CAMS.Common
    └── CAMS.Infrastructure
```

---

## 1. Auctions API

This API manages auctions.

### Key Points:

- **Controllers and endpoints** to create, manage, and end auctions.
- **Application Layer (CQRS with MediatR)**:
  - Commands (StartAuction, PlaceBid, CloseAuction)
  - Queries (GetAuction, SearchAuctions)
- **Data Layer**:
  - InMemoryAuctionRepository
- **Domain Layer**:
  - Auction and Bid entities
  - Domain events (AuctionStartedEvent, BidPlacedEvent, AuctionClosedEvent)

---

## 2. Vehicles API

This API manages vehicle details.

### Key Points:

- **Controllers and endpoints** for creating and searching vehicles.
- **Application Layer (CQRS with MediatR)**:
  - Command (CreateVehicle)
  - Query (SearchVehicles)
- **Data Layer**:
  - InMemoryVehicleRepository
- **Domain Layer**:
  - Vehicle entities (Sedan, SUV, Hatchback, Truck)
  - Factory Method (VehicleFactory)

---

## Communication Between APIs

The APIs use **MassTransit**:

- **Development** uses **In-Memory** messaging.
- **Production** uses **Azure Service Bus**.

### Example:

- `CheckVehicleAvailabilityRequest`: sent from Auctions API.
- `CheckVehicleAvailabilityConsumer`: checks and responds in Vehicles API.

---

## CAMS.Common

Shared parts for both APIs:

- **Exceptions** (AuctionNotFoundException, VehicleNotFoundException, etc.)
- **Middleware** for error handling
- **ResponseResult** for standard responses
- **Configuration** for Service Bus (ServiceBusSettings)

---

## CAMS.Infrastructure

Handles messaging using MassTransit:

- **Consumers and Publishers** for messages
- **MassTransit configuration** (Service Bus and In-Memory)

---

## Libraries Used

- **.NET 8 LTS**
- **MediatR** (CQRS)
- **MassTransit** (Messaging)
- **FluentValidation**
- **Swagger** for API documentation
- **xUnit, Moq, FluentAssertions** for tests (in progress)

---

## Architecture and OO Patterns

- **Clean Architecture** and **Domain-Driven Design (DDD)**
- Clear structure in **Application**, **Domain**, and **Infrastructure**
- **CQRS with MediatR**
- **Domain events** for better organisation
- **Notification Pattern** for clear validations
- **Factory Method** for creating entities easily

---

## How to Run

### Development

Run each API:

```bash
cd src/AuctionsApi/CAMS.Auctions.Api
dotnet run

cd src/VehiclesApi/CAMS.Vehicles.Api
dotnet run
```

### Production

Set this in `appsettings.json`:

```json
"ServiceBusSettings": {
  "ConnectionString": "Azure_Service_Bus_Connection",
  "MaximumRetries": 5,
  "RetryDelaySeconds": 1
},
"MessagingSettings": {
  "ServiceBusQueueName": "queue-name"
}
```

---

## Tests

Automated tests with xUnit, Moq, FluentAssertions (in progress).

---

## Contributions

Contributions are welcome through Pull Requests or Issues.

---

## Conclusion

This version with microservices keeps the project clear and easy to manage, using MassTransit and Azure Service Bus for good communication. For more details about the original version, check the [Car Auction Management System README](https://github.com/vagnerjsmello/car-auction-management-system)

