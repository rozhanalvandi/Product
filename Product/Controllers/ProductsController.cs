using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Product.Data;
using Product.Models;
using Product.Commands;
using Product.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
using System.Linq;


[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(string? createdBy = null)
    {
        var products = string.IsNullOrEmpty(createdBy)
            ? await _context.Products.ToListAsync() 
            : await _context.Products.Where(p => p.CreatedBy == createdBy).ToListAsync();

        return Ok(products);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
    {
        var product = _mapper.Map<Products>(productDto); 
        product.CreatedBy = User.Identity?.Name;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null || product.CreatedBy != User.Identity?.Name)
        {
            return NotFound();
        }

        _mapper.Map(productDto, product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null || product.CreatedBy != User.Identity?.Name)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}