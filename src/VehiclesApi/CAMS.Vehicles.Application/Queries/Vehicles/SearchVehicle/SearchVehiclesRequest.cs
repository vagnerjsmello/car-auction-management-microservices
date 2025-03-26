using CAMS.Vehicles.Domain.Enums;

namespace CAMS.Vehicles.Application.Queries.Vehicles.SearchVehicle;

/// <summary>
/// Query object for searching vehicles using optional filters.
/// </summary>
public class SearchVehiclesRequest
{
    public VehicleType? VehicleType { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public Guid? VehicleId { get; set; }
}
