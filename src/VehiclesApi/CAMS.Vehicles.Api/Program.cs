using CAMS.Common.Middleware;
using CAMS.Vehicles.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add controllers with Newtonsoft, enabling enum serialization as strings.
builder.Services.AddControllers()
    .AddNewtonsoftJson(opts =>
    {
        opts.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    });

// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Vehicles Management API",
        Version = "v1",
        Description = "API dedicated to managing vehicle lifecycle, " +
                      "including creation, updating, and searching vehicles by type, " +
                      "manufacturer, model, and year."
    });
});

// Enable compatibility with Newtonsoft if you're using Swashbuckle.AspNetCore.Newtonsoft
builder.Services.AddSwaggerGenNewtonsoftSupport();

// Register domain-specific services
builder.Services.AddVehicleServices();

// Register MassTransit services (Service bus)
builder.Services.AddMassTransitServices(builder.Configuration, builder.Environment);

var app = builder.Build();

// If in development environment, enable Swagger and SwaggerUI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vehicles Management API");
    });
}

// Custom middleware for exception handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
