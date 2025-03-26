using CAMS.Vehicles.Domain.Enums;
using MediatR;

namespace CAMS.Vehicles.Application.Queries.Vehicles.SearchVehicle;

/// <summary>
/// Query object for searching vehicles using optional filters.
/// </summary>
public class SearchVehiclesQuery : IRequest<SearchVehiclesResponse>
{
    //necessary for model binding
    public SearchVehiclesQuery() { }

    public VehicleType? VehicleType { get; }
    public string? Manufacturer { get; }
    public string? Model { get; }
    public int? Year { get; }
    public Guid? VehicleId { get; set; }


    public SearchVehiclesQuery(SearchVehiclesRequest request)
    {
        VehicleId = request.VehicleId;
        VehicleType = request.VehicleType;
        Manufacturer = request.Manufacturer;
        Model = request.Model;
        Year = request.Year;
    }
}
