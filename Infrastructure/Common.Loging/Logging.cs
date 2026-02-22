using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> LoggerConfiguration =>
            (context, loggerConfiguration) =>
            {
                var env = context.HostingEnvironment;
                var serviceName = env.ApplicationName.Replace(".Api", "").Replace(".API", "");

                loggerConfiguration
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithEnvironmentName()
                    .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                    .Enrich.WithProperty("ServiceName", serviceName)
                    .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                    .Enrich.WithProperty("Version", "1.0.0")
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Console(new JsonFormatter());

                if (env.IsDevelopment())
                {
                    loggerConfiguration.MinimumLevel.Override("Catalog", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Basket", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Discount", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Ordering", LogEventLevel.Debug);
                }

                // Configure Elasticsearch sink
                var elasticUrl = context.Configuration["ElasticConfiguration:Uri"];

                if (!string.IsNullOrEmpty(elasticUrl))
                {
                    loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUrl))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                        IndexFormat = $"ecommerce-{serviceName.ToLower()}-{env.EnvironmentName.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
                        NumberOfShards = 1,
                        NumberOfReplicas = 1,
                        TemplateName = $"ecommerce-{serviceName.ToLower()}-template",
                        OverwriteTemplate = false,
                        TypeName = null, // Use ES 8.x without type names
                        BatchAction = ElasticOpType.Create,
                        InlineFields = true,
                        CustomFormatter = new JsonFormatter(),
                        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog | EmitEventFailureHandling.WriteToFailureSink | EmitEventFailureHandling.RaiseCallback,
                        BufferBaseFilename = "./logs/elasticsearch-buffer",
                        BufferFileSizeLimitBytes = 5242880, // 5MB
                        BufferLogShippingInterval = TimeSpan.FromSeconds(5),
                        RegisterTemplateFailure = RegisterTemplateRecovery.IndexAnyway
                    });
                }
            };
    }
}
