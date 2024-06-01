using Application;
using Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using WebApi.Common;
using WebApi.Common.Extensions;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuring Kestrel to allow uploading big files
builder.Services.Configure<KestrelServerOptions>(options =>
{
	options.Limits.MaxRequestBodySize = int.MaxValue;
});

builder.Services.AddControllers();

#region Configuring IOption patter classes

builder.Services.AddOptions<ApplicationSettings>()
	.BindConfiguration(ApplicationSettings.ConfigurationSection);

#endregion

// Adding httpcontext accessor to get current httpcontext by DI
builder.Services.AddHttpContextAccessor();

// Adding all services from Application part
builder.Services.AddApplication();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCustomExceptionHandlerMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
