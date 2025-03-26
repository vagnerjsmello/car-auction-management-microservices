using CAMS.Auctions.Data.Repositories;
using CAMS.Auctions.Domain.Repositories;
using CAMS.Vehicles.Application.Commands.Vehicles.CreateVehicle;
using CAMS.Vehicles.Data.Repositories;
using CAMS.Vehicles.Domain.Repositories;
using FluentValidation;

namespace CAMS.Vehicles.Api.Extensions;

public static class VehicleDependencyInjectionServices
{
    public static IServiceCollection AddVehicleServices(this IServiceCollection services)
    {
        // Application - CQRS (MediatR)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateVehicleCommand>());

        // Validators
        services.AddScoped<IValidator<CreateVehicleCommand>, CreateVehicleCommandValidator>();

        // Repositories
        services.AddSingleton<IVehicleRepository, InMemoryVehicleRepository>();
        services.AddSingleton<IAuctionRepository, InMemoryAuctionRepository>();


        return services;
    }
}

