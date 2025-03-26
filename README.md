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

## Auctions API

### Key Responsibilities:

#### CAMS.Auctions.Api:
- Provides RESTful endpoints to manage auctions:
  - `POST /api/auctions`: Start a new auction.
  - `POST /api/auctions/{auctionId}/bids`: Place a new bid.
  - `POST /api/auctions/{auctionId}/close`: Close an auction.

#### CAMS.Auctions.Application:
- Uses Commands and Queries (CQRS pattern with MediatR):
  - **Commands:**
    - `StartAuctionCommand`: Checks if vehicle exists and is available, starts a new auction.
    - `PlaceBidCommand`: Places a bid, validates if the new bid is higher.
    - `CloseAuctionCommand`: Closes an active auction and triggers domain events.
  - **Queries:**
    - `GetAuctionQuery`: Retrieves auction details.
    - `SearchAuctionsQuery`: Searches auctions based on criteria.

#### CAMS.Auctions.Data:
- Stores auctions data temporarily using `InMemoryAuctionRepository`.

#### CAMS.Auctions.Domain:
- Defines key entities:
  - **Auction**: Holds auction details, bids, and status (Active/Closed).
  - **Bid**: Details of each bid including amount, bidder ID, and time.
- Manages domain events like:
  - `AuctionStartedEvent`
  - `BidPlacedEvent`
  - `AuctionClosedEvent`

---

## Vehicles API

### Key Responsibilities:

#### CAMS.Vehicles.Api:
- Provides RESTful endpoints to manage vehicles:
  - `POST /api/vehicles`: Register a new vehicle.
  - `GET /api/vehicles`: Search for vehicles.

#### CAMS.Vehicles.Application:
- Uses Commands and Queries (CQRS pattern with MediatR):
  - **Commands:**
    - `CreateVehicleCommand`: Adds a new vehicle to the system.
  - **Queries:**
    - `SearchVehiclesQuery`: Searches vehicles based on criteria.

#### CAMS.Vehicles.Data:
- Stores vehicle data temporarily using `InMemoryVehicleRepository`.

#### CAMS.Vehicles.Domain:
- Defines key entities:
  - **Vehicle (abstract)**: Shared attributes for all vehicles.
  - **Sedan, SUV, Hatchback, Truck**: Specific types of vehicles with unique attributes.
- Provides a Factory Method (`VehicleFactory`) to create vehicles easily and consistently.

---

## Communication Between APIs

The APIs communicate asynchronously using **MassTransit**:

- **Development** uses **In-Memory** messaging.
- **Production** uses **Azure Service Bus** configured in `appsettings.json`.

### Example Communication:
- **Auctions API** sends a `CheckVehicleAvailabilityRequest` message to Vehicles API.
- **Vehicles API** checks availability with `CheckVehicleAvailabilityConsumer` and responds.

---

## CAMS.Common

Shared parts for both APIs:

- **Exceptions**: Provides consistent error handling across both APIs.
- **Middleware**: Handles exceptions and provides uniform responses.
- **ResponseResult**: Standard response format.
- **ServiceBusSettings**: Manages configuration for Azure Service Bus.

---

## CAMS.Infrastructure

Handles messaging configuration and management:

- Configures **MassTransit** for messaging between APIs.
- Implements message Consumers and Publishers.

---

## Libraries Used

- **.NET 8 LTS**
- **MediatR** (CQRS pattern)
- **MassTransit** (Messaging)
- **FluentValidation** (Validation)
- **Swagger** (API documentation)
- **xUnit, Moq, FluentAssertions** (Testing, currently in progress)

---

## Architecture and Object-Oriented Patterns

- **Clean Architecture and Domain-Driven Design (DDD)**: Clear and maintainable project structure.
- **CQRS with MediatR**: Separates commands (actions) from queries (requests for information).
- **Domain Events**: Allows easy communication and action triggering based on changes.
- **Notification Pattern**: Collects and returns validation errors clearly.
- **Factory Method**: Centralises object creation for consistency and simplicity.

---

## How to Run

### Development

Run each API separately:

```bash
cd src/AuctionsApi/CAMS.Auctions.Api
dotnet run

cd src/VehiclesApi/CAMS.Vehicles.Api
dotnet run
```

Test APIs via Swagger:
- Auctions API: `https://localhost:7041/swagger`
- Vehicles API: `https://localhost:7141/swagger`

### Production

Configure Azure Service Bus in `appsettings.json`:

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

Automated tests using xUnit, Moq, FluentAssertions are in development.

---

## Contributions

Contributions welcome through Pull Requests or Issues.

---

## Conclusion

This microservices version improves the project's scalability and maintainability with clear separation of responsibilities and reliable communication using MassTransit and Azure Service Bus. For details about the original version, see the [Car Auction Management System README](https://github.com/vagnerjsmello/car-auction-management-system).

