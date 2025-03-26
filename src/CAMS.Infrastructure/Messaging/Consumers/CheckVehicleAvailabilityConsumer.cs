using CAMS.Infrastructure.Messaging.Vehicles.CheckAvailability;
using CAMS.Vehicles.Domain.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CAMS.Infrastructure.Messaging.Consumers;

/// <summary>
/// Consumer that processes a CheckVehicleAvailabilityRequest message and responds with a CheckVehicleAvailabilityResponse.
/// </summary>
public class CheckVehicleAvailabilityConsumer : IConsumer<CheckVehicleAvailabilityRequest>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ILogger<CheckVehicleAvailabilityConsumer> _logger;

    public CheckVehicleAvailabilityConsumer(
        IVehicleRepository vehicleRepository,
        ILogger<CheckVehicleAvailabilityConsumer> logger)
    {
        _vehicleRepository = vehicleRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CheckVehicleAvailabilityRequest> context)
    {
        _logger.LogInformation($"Received CheckVehicleAvailabilityRequest for VehicleId {context.Message.VehicleId}.");

        // Check if the vehicle exists in the repository
        var vehicle = await _vehicleRepository.GetByIdAsync(context.Message.VehicleId);
        bool isAvailable = vehicle != null;

        var response = new CheckVehicleAvailabilityResponse(VehicleId: context.Message.VehicleId, IsAvailable: isAvailable);

        _logger.LogInformation("Responding with availability: {IsAvailable} for VehicleId {VehicleId}.",
            isAvailable, context.Message.VehicleId);

        // Respond back to the sender
        await context.RespondAsync(response);
    }
}






