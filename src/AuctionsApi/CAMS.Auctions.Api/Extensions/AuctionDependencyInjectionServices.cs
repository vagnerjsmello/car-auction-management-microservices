using CAMS.Auctions.Application.Commands.Auctions.CloseAuction;
using CAMS.Auctions.Application.Commands.Auctions.PlaceBid;
using CAMS.Auctions.Application.Commands.Auctions.StartAuction;
using CAMS.Auctions.Data.Repositories;
using CAMS.Auctions.Domain.Repositories;
using CAMS.Vehicles.Data.Repositories;
using CAMS.Vehicles.Domain.Repositories;
using FluentValidation;

namespace CAMS.Auctions.Api.Extensions;

public static class AuctionDependencyInjectionServices
{
    public static IServiceCollection AddAuctionServices(this IServiceCollection services)
    {
        // CQRS Auctions
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<StartAuctionCommand>());

        // Validators
        services.AddScoped<IValidator<StartAuctionCommand>, StartAuctionCommandValidator>();
        services.AddScoped<IValidator<PlaceBidCommand>, PlaceBidCommandValidator>();
        services.AddScoped<IValidator<CloseAuctionCommand>, CloseAuctionCommandValidator>();

        // Repositories
        services.AddSingleton<IVehicleRepository, InMemoryVehicleRepository>();
        services.AddSingleton<IAuctionRepository, InMemoryAuctionRepository>();



        return services;
    }
}
