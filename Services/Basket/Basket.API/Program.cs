using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Mapper;
using Basket.Core.Repositories;
using Basket.Infrasture.Repositories;
using Common.Logging;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.OpenApi;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog(Logging.LoggerConfiguration);

builder.Services.AddControllers();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(typeof(BasketMappingProfile).Assembly);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly(),
        Assembly.GetAssembly(typeof(CreateShoppingCartCommand))!
    );
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddGrpcClient<Discount.Grpc.Protos.DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"] ?? throw new InvalidOperationException("GrpcSettings:DiscountUrl is not configured."));
});

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]
            ?? throw new InvalidOperationException("EventBusSettings:HostAddress is not configured."));
    });
});
builder.Services.AddMassTransitHostedService();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Basket.Api",
        Version = "v1",
        Description = "This is Basket Service from E-Commerce ApplicatIon ",
        Contact = new OpenApiContact
        {
            Name = "Bilal-Altiti",
            Email = "bilalaltiti@hotmail.com",
            Url = new Uri("https://sooquk.com/ar"),
        }
    });
});

// Redis Configuration
var redisConnectionString = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
if (string.IsNullOrEmpty(redisConnectionString))
{
    throw new InvalidOperationException("Redis connection string is not configured. Please set CacheSettings:ConnectionString in appsettings.json");
}

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseDeveloperExceptionPage();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");
        c.RoutePrefix = "swagger";
    }); app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
