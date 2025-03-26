namespace CAMS.Infrastructure.Messaging.Vehicles.CheckAvailability
{
    /// <summary>
    /// Request object for checking if a vehicle is available.
    /// </summary>
    public record CheckVehicleAvailabilityRequest(Guid VehicleId);
}
