using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using OrderService.Models;

var builder = WebApplication.CreateBuilder(args);

// RabbitMQ-Client als Singleton registrieren
builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory() { HostName = "rabbitmq" });

// DbContext für SQL Server registrieren
builder.Services.AddDbContext<OrderContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Füge Controller hinzu
builder.Services.AddControllers();

var app = builder.Build();

// Migrationen anwenden
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

