﻿using CAMS.Common.Exceptions;
using CAMS.Common.ResponseApi;
using CAMS.Vehicles.Domain.Factories;
using CAMS.Vehicles.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CAMS.Vehicles.Application.Commands.Vehicles.CreateVehicle;

/// <summary>
/// Handler responsible for processing the creation of a vehicle command.
/// </summary>
public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, ResponseResult<CreateVehicleResponse>>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IValidator<CreateVehicleCommand> _validator;
    private readonly ILogger<CreateVehicleCommandHandler> _logger;

    public CreateVehicleCommandHandler(
        ILogger<CreateVehicleCommandHandler> logger,
        IVehicleRepository vehicleRepository,
        IValidator<CreateVehicleCommand> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<ResponseResult<CreateVehicleResponse>> Handle(CreateVehicleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateVehicleCommand for Vehicle ID: {VehicleId}", command.Id);

        // Validate the command
        ValidationResult validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning($"Validation failed for {command.GetType().Name}: {validationResult.Errors}");
            return ResponseResult<CreateVehicleResponse>.Fail(validationResult);
        }

        // Check for duplicate vehicle
        var vehicle = await _vehicleRepository.GetByIdAsync(command.Id);
        if (vehicle is not null)
        {
            var ex = new VehicleAlreadyExistsException(command.Id);
            _logger.LogWarning(ex.Message);
            throw ex;
        }

        // Create vehicle using the factory
        vehicle = VehicleFactory.CreateVehicle(
            command.VehicleType,
            command.Id,
            command.Manufacturer,
            command.Model,
            command.Year,
            command.StartingBid,
            command.NumberOfDoors,
            command.NumberOfSeats,
            command.LoadCapacity
        );

        await _vehicleRepository.AddAsync(vehicle);
        _logger.LogInformation("Vehicle created successfully with ID: {VehicleId}", command.Id);

        return ResponseResult<CreateVehicleResponse>.Success(new CreateVehicleResponse(vehicle));
    }
}