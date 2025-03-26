using CAMS.Common.Exceptions;
using CAMS.Vehicles.Domain.Entities;
using CAMS.Vehicles.Domain.Repositories;
using System.Collections.Concurrent;

namespace CAMS.Vehicles.Data.Repositories;

/// <summary>
/// In-memory implementation of IVehicleRepository.
/// </summary>
public class InMemoryVehicleRepository : IVehicleRepository
{
    private readonly ConcurrentDictionary<Guid, Vehicle> _vehicles = new ConcurrentDictionary<Guid, Vehicle>();

    public async Task AddAsync(Vehicle vehicle)
    {
        if (!_vehicles.TryAdd(vehicle.Id, vehicle))
        {
            throw new VehicleAlreadyExistsException(vehicle.Id);
        }
        await Task.CompletedTask;
    }

    public async Task<Vehicle?> GetByIdAsync(Guid id)
    {        
        _vehicles.TryGetValue(id, out var vehicle);
        return await Task.FromResult(vehicle);
    }


    public async Task<IEnumerable<Vehicle>> Search(Func<Vehicle, bool> predicate)
    {
        var result = _vehicles.Values.Where(predicate);
        return await Task.FromResult(result);
    }
}
