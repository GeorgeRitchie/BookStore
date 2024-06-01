using Application;
using Application.Common.Interfaces.Services;
using Infrastructure.Common;
using Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using WebApi.Common;
using WebApi.Common.Extensions;
using WebApi.Services;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using AutoMapper;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Configuring Kestrel to allow uploading big files
builder.Services.Configure<KestrelServerOptions>(options =>
{
	options.Limits.MaxRequestBodySize = int.MaxValue;
});

builder.Services.AddControllers();

#region Configuring IOption patter classes

builder.Services.AddOptions<WebApiSettings>()
	.BindConfiguration(WebApiSettings.ConfigurationSection);

builder.Services.AddOptions<InfrastructureSettings>()
	.BindConfiguration(InfrastructureSettings.ConfigurationSection)
	.PostConfigure<IWebHostEnvironment>((options, env) =>
	{
		options.Environment = env.EnvironmentName;
		options.IsDevelopmentEnvironment = env.IsDevelopment();
	});

#endregion

// Adding httpcontext accessor to get current httpcontext by DI
builder.Services.AddHttpContextAccessor();

// Adding all services from Application part
builder.Services.AddApplication();

// Adding all services from Infrastructure part
builder.Services.AddInfrastructure();

// Adding Swagger. Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

// Adding Serilog
builder.Services.AddSerilogStuff();

// Adding CORS policies
builder.Services.AddCors(opt =>
{
	// TODO __##__ Warning this policy is for development ONLY!
	opt.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyHeader();
		policy.AllowAnyMethod();
		policy.AllowAnyOrigin();
	});
});

// Adding ApiVersioning
builder.Services.AddAPIVersioning();

// Adding authentication and authorization by JWT token
builder.Services.AddAuthenticationByJwtToken();

#region Custom Service Registration here

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

#endregion

var app = builder.Build();

var webApiSettings = app.Services.GetRequiredService<IOptions<WebApiSettings>>().Value;

DoActionsBeforeStartTheProgram(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || webApiSettings.EnableSwaggerUI)
{
	var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

	app.UseSwagger();
	app.UseSwaggerUI(opt =>
	{
		// build a swagger endpoint for each discovered API version
		foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
		{
			opt.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
		}
		opt.InjectStylesheet("/swagger-ui/SwaggerDark.css");
	});
}

app.UseCustomExceptionHandlerMiddleware();

if (app.Environment.IsProduction() || webApiSettings.EnableHttpsRedirection)
{
	app.UseHsts();
	app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void DoActionsBeforeStartTheProgram(WebApplication app)
{
	using (var scope = app.Services.CreateScope())
	{
		var serviceProvider = scope.ServiceProvider;

		try
		{
			var autoMapper = serviceProvider.GetRequiredService<IMapper>();

			// Calling Db Initializer
			var repository = serviceProvider.GetRequiredService<IAppDb>();
			AppDbInitializer.Initialize(repository, autoMapper);

			// any code to do before start program
		}
		catch (Exception ex)
		{
			Log.Fatal(ex, "An error occurred while app initialization.");

			throw;
		}
	}
}
