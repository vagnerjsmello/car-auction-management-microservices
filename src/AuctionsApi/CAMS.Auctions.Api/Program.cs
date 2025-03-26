using CAMS.Auctions.Api.Extensions;
using CAMS.Common.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Auctions Management API",
        Version = "v1",
        Description = "API focused on creating, starting, bidding on, and closing auctions for registered vehicles."
    });


});
builder.Services.AddSwaggerGenNewtonsoftSupport();

//Add Auctions services
builder.Services.AddAuctionServices();

// Register MassTransit services (Service bus)
builder.Services.AddMassTransitServices(builder.Configuration, builder.Environment);

//Add NewtonSoftJson to handle Enum serialization
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(opts =>
    {
        opts.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {        
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auctions Management API");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
