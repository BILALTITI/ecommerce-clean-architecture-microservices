using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data.Context;
using Catalog.Infrastructure.Repositories;
using Common.Logging;
using Discount.Core.Entites;
using DnsClient; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Serilog;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;


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


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(


    Options =>
    {
        Options.Authority = "https://host.docker.internal:9009";
        Options.RequireHttpsMetadata = true;

        Options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {

            ValidateIssuer = true,
            ValidIssuer= "http://localhost:9009",
          ValidateAudience=true,
          ValidAudience= "Catalog",
          ValidateIssuerSigningKey=true,
          ClockSkew=TimeSpan.Zero

        };

        Options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback=(message,cart,chain,error)=>true
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanRead", policy =>
        policy.RequireClaim("scope", "catalog.read"));
});
builder.Services.AddScoped<iCatalogContext,CatalogContext>();
builder.Services.AddScoped<IBrandRepositreis,ProductRepositoriy>();
builder.Services.AddScoped<ITypeRepositreis,ProductRepositoriy>();
builder.Services.AddScoped<IProductRepositreis,ProductRepositoriy>();

var userpolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();


builder.Services.AddControllers(config => config.Filters.Add(new AuthorizeFilter(userpolicy)));
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
     // BEFORE UseSwagger / routing
    app.Use((ctx, next) =>
    {
        if (ctx.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var p) && !string.IsNullOrEmpty(p))
            ctx.Request.PathBase = p.ToString();   // e.g., "/catalog"
        return next();
    });

    app.UseSwagger(c =>
    {
        // Make the OpenAPI "servers" base path match the prefix so Try it out uses /catalog/...
        c.PreSerializeFilters.Add((doc, req) =>
        {
            var prefix = req.Headers["X-Forwarded-Prefix"].FirstOrDefault();
            if (!string.IsNullOrEmpty(prefix))
                doc.Servers = new List<Microsoft.OpenApi.OpenApiServer>
            { new() { Url = prefix } };
        });
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Catalog.API v1"); // relative path (no leading '/')
        c.RoutePrefix = "swagger";
    });
    app.MapOpenApi();
}

app.UseSerilogRequestLogging(); // This will log all HTTP requests automatically

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
