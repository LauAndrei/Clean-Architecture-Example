using NET8_CleanArchitecture_Azure.Middlewares;
using Serilog;

namespace NET8_CleanArchitecture_Azure.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom
                .Configuration(context.Configuration);
        });

        builder.Services.AddAuthentication();
    }
}