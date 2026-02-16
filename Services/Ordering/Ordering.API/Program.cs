using EventBusMessages.Common;
using MassTransit;
using MassTransit.Testing;
using Microsoft.OpenApi;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog(Common.Logging.Logging.LoggerConfiguration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Order.Api",
        Version = "v1",
        Description = "Ordering Service from E-Commerce Application",
        Contact = new OpenApiContact
        {
            Name = "Bilal Altiti",
            Email = "bilalaltiti@hotmail.com",
            Url = new Uri("https://sooquk.com/ar"),
        }
    });
});
builder.Services.AddApplicationServices();




builder.Services.AddInfraService(builder.Configuration);
builder.Services.AddScoped<BaksetOrderingConsumer>();
builder.Services.AddScoped<BaksetOrderingConsumerV2>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BaksetOrderingConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]
            ?? throw new InvalidOperationException("EventBusSettings:HostAddress is not configured."));
        cfg.ReceiveEndpoint(EventBasketConstent.BasketCheckoutQueue, e =>
        {
            e.ConfigureConsumer<BaksetOrderingConsumer>(context);
        });
       
        // V2
        cfg.ReceiveEndpoint(EventBasketConstent.BasketCheckoutQueueV2, e =>
        {
            e.ConfigureConsumer<BaksetOrderingConsumerV2>(context);
        });
    });
});
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();


app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
        c.RoutePrefix = "swagger";
    });
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
