using Common.Logging;
using Discount.API.Services;
using Discount.Application.Commands;
using Discount.Application.Mapper;
using Discount.Core.Entites;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Discount.Infrastructure;
using Discount.Infrastructure.Extensions;
using Microsoft.OpenApi;
using Serilog;
 using System.Reflection;
 
var builder = WebApplication.CreateBuilder(args);

// -------------------- Services --------------------

builder.Services.AddControllers();
builder.Host.UseSerilog(Logging.LoggerConfiguration);

builder.Services.AddAutoMapper(typeof(DiscountProfile).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly(),
        Assembly.GetAssembly(typeof(CreateDiscountCommand))!
    );
});

builder.Services.AddScoped<IdiscountRepository, DiscountRepository>();

builder.Services.AddGrpc();

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
        Title = "Discount.Api",
        Version = "v1",
        Description = "Discount Service from E-Commerce Application",
        Contact = new OpenApiContact
        {
            Name = "Bilal Altiti",
            Email = "bilalaltiti@hotmail.com",
            Url = new Uri("https://sooquk.com/ar"),
        }
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();


// -------------------- Middleware --------------------

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Discount API V1");
        c.RoutePrefix = "swagger";
    });

    app.MapOpenApi();
}

app.UseRouting();
app.UseAuthorization();

// -------------------- Endpoints --------------------

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<DiscountService>();
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with grpc endpoints must be made through a grpc client");
    });
});
// -------------------- Startup Tasks --------------------

app.MigrateDataBase<Program>();

app.Run();
