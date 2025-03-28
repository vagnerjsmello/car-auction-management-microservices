﻿using CAMS.Vehicles.Domain.Entities;

namespace CAMS.Vehicles.Domain.Repositories;

public interface IVehicleRepository
{
    Task AddAsync(Vehicle vehicle);
    Task<Vehicle> GetByIdAsync(Guid id);
    Task<IEnumerable<Vehicle>> Search(Func<Vehicle, bool> predicate);
}
