using Xunit;
using Moq;
using AutoMapper;
using Product.Domain.Models;
using Product.Application.DTOs; 
using Product.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Product.Application.Commands;
using System;
using System.Threading.Tasks;
using Product.Application.Handlers;
using Product.Infrastructure.Repository;

namespace Product.Tests.Handlers
{
   public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Add_Product_To_Database()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ProductDbTest")
            .Options;

        await using var context = new AppDbContext(options);
        
        context.Database.EnsureCreated();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<Products>(It.IsAny<CreateProductCommand>()))
                  .Returns((CreateProductCommand command) => new Products
                  {
                      Name = command.Name,
                      ManufactureEmail = command.ManufactureEmail,
                      ManufacturePhone = command.ManufacturePhone,
                      ProduceDate = command.ProduceDate,
                      IsAvailable = command.IsAvailable
                  });

        var productRepository = new ProductRepository(context);
        var handler = new CreateProductCommandHandler(productRepository, mapperMock.Object);

        var command = new CreateProductCommand
        {
            Name = "Test",
            ManufactureEmail = "test@example.com",
            ManufacturePhone = "1234567890",
            ProduceDate = new DateTime(2025, 7, 10),
            IsAvailable = true
        };

        var result = await handler.Handle(command, default);

        Assert.NotNull(result);
        Assert.Single(context.Products); 
    }
}
}
