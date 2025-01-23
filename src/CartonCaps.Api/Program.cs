using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using CartonCaps.Api.Services;
using CartonCaps.Api.Infrastructure.Swagger;

namespace CartonCaps.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<IReferralService, ReferralService>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Carton Caps Referral API",
                Version = "v1",
                Description = @"API endpoints for the Carton Caps referral system.

Key Features:
- Retrieve and share referral codes
- Generate shareable deep links for multiple sharing methods
- Track referral status and history
- Validate referral codes during new user signup"
            });

            // Add operation filters for better documentation
            c.EnableAnnotations();
            
            // Add response examples
            c.UseInlineDefinitionsForEnums();

            // Add request/response examples
            c.SchemaFilter<ExampleSchemaFilter>();
            
            // Add XML comments
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // Add custom schema mappings and examples
            c.MapType<DateTimeOffset>(() => new OpenApiSchema 
            { 
                Type = "string",
                Format = "date-time",
                Example = new Microsoft.OpenApi.Any.OpenApiString(DateTimeOffset.UtcNow.ToString("o"))
            });
        });

        // Add rate limiting
        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            
            options.AddFixedWindowLimiter("fixed", options =>
            {
                options.Window = TimeSpan.FromMinutes(1);
                options.PermitLimit = 20;
                options.QueueLimit = 2;
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
                    };
                });
            });
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Carton Caps Referral API v1");
                c.DocumentTitle = "Carton Caps Referral API Documentation";
                c.DefaultModelsExpandDepth(2);
                c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.ShowExtensions();
            });
        }

        app.UseHttpsRedirection();
        app.UseRateLimiter();
        app.MapControllers();

        app.Run();
    }
}
