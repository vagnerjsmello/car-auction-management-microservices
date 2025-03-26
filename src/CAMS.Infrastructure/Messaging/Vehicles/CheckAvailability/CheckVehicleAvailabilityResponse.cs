namespace CAMS.Infrastructure.Messaging.Vehicles.CheckAvailability
{
    /// <summary>
    /// Response object for checking vehicle availability.
    /// </summary>
    public record CheckVehicleAvailabilityResponse(Guid VehicleId, bool IsAvailable);
}



