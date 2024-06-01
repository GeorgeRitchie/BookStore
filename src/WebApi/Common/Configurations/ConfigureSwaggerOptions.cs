using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace WebApi.Common.Configurations
{
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		private readonly IApiVersionDescriptionProvider provider;

		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

		public void Configure(SwaggerGenOptions options)
		{
			foreach (var description in provider.ApiVersionDescriptions)
			{
				var apiVersion = description.ApiVersion.ToString();

				// configure to add xml commets into swagger documentation
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);

				// configure to add general info about program in swagger UI
				options.SwaggerDoc(description.GroupName,
				new OpenApiInfo
				{
					Title = $"Api {apiVersion}",
					Version = apiVersion,
					Description = "Api to interact with server.",
					TermsOfService = new Uri("https://example.com/terms"),  // TODO __##__ Make it real address to real terms of service
					Contact = new OpenApiContact                            // TODO __##__ Make it real contact to company
					{
						Name = "Sharifjon Abdulloev",
						Email = "sharifjonabdulloev@outlook.com",
						Url = new Uri("https://t.me/SharifjonAbdulloev")
					},
					License = new OpenApiLicense                            // TODO __##__ Make it real license
					{
						Name = "Example License",
						Url = new Uri("https://example.com/license")
					}
				});

				options.AddSecurityDefinition($"AuthToken {apiVersion}",
					new OpenApiSecurityScheme
					{
						In = ParameterLocation.Header,
						Type = SecuritySchemeType.Http,
						BearerFormat = "JWT",
						Scheme = "Bearer",
						Name = "Authorization",
						Description = "Authorization token"
					});

				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = $"AuthToken {apiVersion}"
							}
						},
						new string[] { }
					}
				});

				options.CustomOperationIds(apiDescription =>
					apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
						? methodInfo.Name
						: null);

				// Resolve schemaId conflict for classes with the same name in different namespaces
				options.CustomSchemaIds(type => type.FullName);
			}
		}
	}
}
