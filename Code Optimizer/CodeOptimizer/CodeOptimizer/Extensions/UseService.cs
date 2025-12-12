using CodeOptimizer.Infrastructure.Middlewares;

namespace CodeOptimizer.API.Extensions
{
    public static class UseService
    {
        extension(WebApplication app)
        {
            public void UseServiceDefaults()
            {
                app.UseMiddleware<GlobalExceptionMiddleware>();

                app.UseRouting();

                app.UseCors("AllowAll");
                app.UseHttpsRedirection();

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseMiddleware<RateLimitMiddleware>();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CodeOptimizer API V1");
                });

                app.MapPrometheusScrapingEndpoint();
            }
        }
    }
}
