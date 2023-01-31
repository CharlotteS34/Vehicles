using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vehicles.Business.Services;
using Vehicles.Common.Repositories;
using Vehicles.Common.Services;
using Vehicles.Repository;
using Vehicles.Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<VehicleDbContext>(options => options.UseSqlServer(@"Server=DESKTOP-4BU15UU\MSSQLSERVER03;Database=Vehicles;TrustServerCertificate=True;Integrated Security=SSPI;"));
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

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
