﻿using CAMS.Vehicles.Domain.Enums;

namespace CAMS.Vehicles.Domain.Entities;

/// <summary>
/// Represents a Hatchback vehicle with a specified number of doors.
/// </summary>
public class Hatchback : Vehicle
{
    public int NumberOfDoors { get; }
    public override VehicleType Type => VehicleType.Hatchback;

    public Hatchback(Guid id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
        : base(id, manufacturer, model, year, startingBid)
    {
        if (numberOfDoors <= 0)
            throw new ArgumentException("NumberOfDoors must be greater than zero.", nameof(numberOfDoors));

        NumberOfDoors = numberOfDoors;
    }
}
