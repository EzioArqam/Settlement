
using Microsoft.EntityFrameworkCore;
using SettlementService.Data;
using SettlementService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Adding DB Context

builder.Services.AddDbContext<BookingsDbContext>(options =>
    options.UseInMemoryDatabase("BookingDatabase"));

// Adding Scoped Repositories for Booking

builder.Services.AddScoped<IBookingRepository, BookingRepository>();

//Adding Automapper

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
