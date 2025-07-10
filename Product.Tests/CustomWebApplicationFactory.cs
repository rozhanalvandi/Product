using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity; 
using Product.Data;
using Product.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Product.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection? _testConnection; 

        public CustomWebApplicationFactory()
        {
            if (_testConnection == null) 
            {
                _testConnection = new SqliteConnection($"DataSource=file:{Guid.NewGuid().ToString()}?mode=memory&cache=shared");
                _testConnection.Open();
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlite(_testConnection!);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var dbContext = scopedServices.GetRequiredService<AppDbContext>();
                    var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>(); // UserManager is still resolved and used

                    dbContext.Database.EnsureDeleted();
                    dbContext.Database.EnsureCreated();

                    Task.Run(async () =>
                    {
                        IdentityUser testUser = await userManager.FindByNameAsync("testUser");
                        if (testUser == null)
                        {
                            testUser = new IdentityUser { UserName = "testUser", Email = "test@example.com" };
                            var result = await userManager.CreateAsync(testUser, "TestPassword1!");
                            if (!result.Succeeded)
                            {
                                throw new InvalidOperationException($"Failed to seed test user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                            }
                            Console.WriteLine("Test user 'testUser' created successfully in test DB.");
                        }
                        else
                        {
                            Console.WriteLine("Test user 'testUser' already exists in test DB.");
                        }

                        dbContext.Products.AddRange(
                            new Products
                            {
                                Name = "Test Product 1",
                                ManufactureEmail = "manu1@example.com",
                                ManufacturePhone = "111-222-3333",
                                ProduceDate = DateTime.Today.AddDays(-10),
                                IsAvailable = true,
                                CreatedBy = testUser.UserName! 
                            },
                            new Products
                            {
                                Name = "Test Product 2",
                                ManufactureEmail = "manu2@example.com",
                                ManufacturePhone = "444-555-6666",
                                ProduceDate = DateTime.Today.AddDays(-5),
                                IsAvailable = false,
                                CreatedBy = "anotherUser"
                            }
                        );
                        await dbContext.SaveChangesAsync();
                        Console.WriteLine("Test products seeded successfully in test DB.");
                    }).Wait();
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _testConnection?.Close();
                _testConnection?.Dispose();
                _testConnection = null;
            }
        }
    }
}