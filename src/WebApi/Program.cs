using WebApi.Common;
using WebApi.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Configuring IOption patter classes

builder.Services.AddOptions<ApplicationSettings>()
	.BindConfiguration(ApplicationSettings.ConfigurationSection);

#endregion

// Adding Swagger. Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

// Adding Serilog
builder.Services.AddSerilogStuff();

// Adding ApiVersioning
builder.Services.AddAPIVersioning();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();