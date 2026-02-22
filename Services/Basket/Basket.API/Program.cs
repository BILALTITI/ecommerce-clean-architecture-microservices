using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Mapper;
using Basket.Core.Repositories;
using Basket.Infrasture.Repositories;
using Common.Logging;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
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



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(


    Options =>
    {
        Options.Authority = "https://host.docker.internal:9009";
        Options.RequireHttpsMetadata = true;

        Options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {

            ValidateIssuer = true,
            ValidIssuer = "http://localhost:9009",
            ValidateAudience = true,
            ValidAudience = "Basket",
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero

        };

        Options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cart, chain, error) => true
        };
        Options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("❌ Authentication Failed");
                Console.WriteLine($"Message: {context.Exception.Message}");
                Console.WriteLine($"StackTrace: {context.Exception.StackTrace}");

                return Task.CompletedTask;
            }
        };

    }

);
 
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();

var userpolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();


builder.Services.AddControllers(config => config.Filters.Add(new AuthorizeFilter(userpolicy)));
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

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
}).AddApiExplorer(options =>
{

    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;



});
builder.Services.AddEndpointsApiExplorer();

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
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Basket.Api",
        Version = "v2",
        Description = "This is Basket Service from E-Commerce Application V2 ",
        Contact = new OpenApiContact
        {
            Name = "Bilal-Altiti",
            Email = "bilalaltiti@hotmail.com",
            Url = new Uri("https://sooquk.com/ar"),
        }
    });

    options.DocInclusionPredicate((version, apiDescription) =>
    {
        if (!apiDescription.TryGetMethodInfo(out MethodInfo methodInfo))
        {
            return false;
        }
        
        var versions = methodInfo.DeclaringType?
            .GetCustomAttributes(true)
            .OfType<Asp.Versioning.ApiVersionAttribute>()
            .SelectMany(attr => attr.Versions)
            .ToList();
        
        if (versions == null || !versions.Any())
        {
            // If no version attribute, it's the default version (v1)
            return version == "v1";
        }
        
        // Check if any version matches the Swagger doc version
        return versions.Any(v => 
        {
            var versionString = v.ToString();
            // Handle both "2" and "2.0" formats
            var normalized = versionString.Replace(".0", "");
            return $"v{normalized}" == version || $"v{versionString}" == version;
        });
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
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Basket API V2");
        c.RoutePrefix = "swagger";
    }); app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
