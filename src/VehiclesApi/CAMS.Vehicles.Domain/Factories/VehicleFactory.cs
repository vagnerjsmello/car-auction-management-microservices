﻿using CAMS.Vehicles.Domain.Entities;
using CAMS.Vehicles.Domain.Enums;

namespace CAMS.Vehicles.Domain.Factories;

public static class VehicleFactory
{
    public static Vehicle CreateVehicle(
        VehicleType vehicleType,
        Guid id,
        string manufacturer,
        string model,
        int year,
        decimal startingBid,
        int numberOfDoors = 0,
        int numberOfSeats = 0,
        double loadCapacity = 0
        )
    {
        return vehicleType switch
        {
            VehicleType.Hatchback => new Hatchback(id, manufacturer, model, year, startingBid, numberOfDoors),
            VehicleType.Sedan => new Sedan(id, manufacturer, model, year, startingBid, numberOfDoors),
            VehicleType.SUV => new SUV(id, manufacturer, model, year, startingBid, numberOfSeats),
            VehicleType.Truck => new Truck(id, manufacturer, model, year, startingBid, loadCapacity),
            _ => throw new ArgumentOutOfRangeException(nameof(Type), $"Invalid vehicle type: {vehicleType}")
        };
    }
}