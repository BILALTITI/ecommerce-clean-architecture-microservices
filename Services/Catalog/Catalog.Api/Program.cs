using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data.Context;
using Catalog.Infrastructure.Repositories;
using Common.Logging;
using DnsClient;
using Microsoft.OpenApi;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Host.UseSerilog(Common.Logging.Logging.LoggerConfiguration);

builder .Services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly(),
        Assembly.GetAssembly(typeof(GetProductByIdQuery))!
    );
});


builder.Services.AddScoped<iCatalogContext,CatalogContext>();
builder.Services.AddScoped<IBrandRepositreis,ProductRepositoriy>();
builder.Services.AddScoped<ITypeRepositreis,ProductRepositoriy>();
builder.Services.AddScoped<IProductRepositreis,ProductRepositoriy>(); 

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning. ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalog.Api",
        Version = "v1",
        Description = "This is Catalog Service from E-Commerce ApplicatIon ",
        Contact = new OpenApiContact
        {
            Name = "Bilal-Altiti",
            Email = "bilalaltiti@hotmail.com",
            Url = new Uri("https://sooquk.com/ar"),
         }
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API V1");
        c.RoutePrefix = "swagger";
    });
    app.MapOpenApi();
}

app.UseSerilogRequestLogging(); // This will log all HTTP requests automatically

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
