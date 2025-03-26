using CAMS.Common.Settings;
using CAMS.Infrastructure.Events.MassTransit;
using CAMS.Infrastructure.Messaging.Consumers;
using CAMS.Infrastructure.Messaging.Events;
using CAMS.Infrastructure.Messaging.Vehicles.CheckAvailability;
using MassTransit;


namespace CAMS.Auctions.Api.Extensions;

public static class MassTransitServices
{
    public static IServiceCollection AddMassTransitServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBusSettings"));
        services.Configure<MessagingSettings>(configuration.GetSection("MessagingSettings"));

        var serviceBusSettings = configuration.GetSection("ServiceBusSettings").Get<ServiceBusSettings>();
        var messagingSettings = configuration.GetSection("MessagingSettings").Get<MessagingSettings>();

        services.AddScoped<IDomainEventPublisher, MassTransitDomainEventPublisher>();

        services.AddMassTransit(mt =>
        {
            mt.SetKebabCaseEndpointNameFormatter();

            // Consumers e Clients
            mt.AddConsumer<CheckVehicleAvailabilityConsumer>();
            mt.AddRequestClient<CheckVehicleAvailabilityRequest>();

            if (env.IsProduction())
            {
                // Azure Service Bus em produção
                mt.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(serviceBusSettings.ConnectionString);

                    cfg.ReceiveEndpoint(messagingSettings.ServiceBusQueueName, e =>
                    {
                        e.ConfigureConsumer<CheckVehicleAvailabilityConsumer>(context);
                    });

                    cfg.UseMessageRetry(r =>
                        r.Interval(serviceBusSettings.MaximumRetries, TimeSpan.FromSeconds(serviceBusSettings.RetryDelaySeconds)));
                });
            }
            else
            {
                // In-Memory para dev/test
                mt.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            }
        });

        return services;
    }
}
