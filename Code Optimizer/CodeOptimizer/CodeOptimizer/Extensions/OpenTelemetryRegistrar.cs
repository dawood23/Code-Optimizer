using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CodeOptimizer.API.Extensions
{
    public static class OpenTelemetryRegistrar
    {
        extension(IServiceCollection services) 
        {
            public void AddOpenTelemetryService()
            {
                services.AddOpenTelemetry()
                    .ConfigureResource(resource => resource.AddService("CodeOptimizer"))
                    .WithMetrics(metrics =>
                    {
                        metrics.AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter()
                        .AddMeter("codeoptimizer.metrics")
                        .AddPrometheusExporter();

                    })
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter();
                });
            }
        }
    }
}
