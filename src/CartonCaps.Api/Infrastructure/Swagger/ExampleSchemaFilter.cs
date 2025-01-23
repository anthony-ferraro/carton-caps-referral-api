using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using CartonCaps.Api.Models;
using CartonCaps.Api.Models.Enums;
using CartonCaps.Api.Models.Responses;

namespace CartonCaps.Api.Infrastructure.Swagger;

/// <summary>
/// Adds examples to the OpenAPI schema documentation.
/// </summary>
public class ExampleSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(ReferralCode))
        {
            schema.Example = new OpenApiObject
            {
                ["code"] = new OpenApiString("XY7G4D")
            };
        }
        else if (context.Type == typeof(GenerateReferralLinkResponse))
        {
            schema.Example = new OpenApiObject
            {
                ["referralLink"] = new OpenApiString("https://cartoncaps.link/abfilefa90p?referral_code=XY7G4D")
            };
        }
        else if (context.Type == typeof(ReferralValidation))
        {
            schema.Example = new OpenApiObject
            {
                ["isValid"] = new OpenApiBoolean(true),
                ["referrerSchool"] = new OpenApiString("Lincoln Elementary"),
                ["onboardingFlow"] = new OpenApiString("referred_user")
            };
        }
        else if (context.Type == typeof(ReferralHistory))
        {
            schema.Example = new OpenApiObject
            {
                ["referredUserName"] = new OpenApiString("Jenny S."),
                ["status"] = new OpenApiString("complete"),
                ["shareMethod"] = new OpenApiString("text"),
                ["createdAt"] = new OpenApiString(DateTimeOffset.UtcNow.AddDays(-5).ToString("o")),
                ["completedAt"] = new OpenApiString(DateTimeOffset.UtcNow.AddDays(-4).ToString("o")),
                ["referrerSchool"] = new OpenApiString("Lincoln Elementary")
            };
        }
        else if (context.Type == typeof(Error))
        {
            schema.Example = new OpenApiObject
            {
                ["code"] = new OpenApiString("invalid_code"),
                ["message"] = new OpenApiString("The specified referral code was not found")
            };
        }
    }
} 