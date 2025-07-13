using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Product.Application.Validators;
using Product.Domain.Models;
using Product.Application.DTOs;
using System.Net.Http.Json;
namespace Product.Tests.Controllers
{
    public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ProductsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
     public async Task Get_All_Products_Should_Return_OK()
    {
            var token = await GetJwtToken(); 

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/products");
           
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var products = await response.Content.ReadFromJsonAsync<IEnumerable<Products>>();
            Assert.NotNull(products);
            Assert.NotEmpty(products); 
    }
   private async Task<string> GetJwtToken()
{
    var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new 
    { 
        Username = "testUser", 
        Password = "TestPassword1!" 
    });

    loginResponse.EnsureSuccessStatusCode();

    var tokenResponse = await loginResponse.Content.ReadFromJsonAsync<TokenResponse>();
    return tokenResponse!.Token; 
}
}
}
