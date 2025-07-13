using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using Product.Infrastructure.Data;
using Product.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Product.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection;

    public CustomWebApplicationFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();

                db.Database.Migrate();

                db.Products.Add(new Products
                {
                    Name = "Test Product 1",
                    ManufactureEmail = "test1@example.com",
                    ProduceDate = DateTime.UtcNow,
                    IsAvailable = true
                });
                db.Products.Add(new Products
                {
                    Name = "Test Product 2",
                    ManufactureEmail = "test2@example.com",
                    ProduceDate = DateTime.UtcNow,
                    IsAvailable = false
                });

                db.SaveChanges();
            }
        });
    }


    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
}