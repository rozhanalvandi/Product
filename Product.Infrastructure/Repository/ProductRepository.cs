using Product.Domain.Models;
using Product.Domain.Interfaces;
using Product.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Product.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Products>> GetAllAsync(string? createdBy = null)
    {
        return string.IsNullOrEmpty(createdBy)
            ? await _context.Products.ToListAsync()
            : await _context.Products.Where(p => p.CreatedBy == createdBy).ToListAsync();
    }

    public async Task<Products?> GetByIdAsync(int id) => await _context.Products.FindAsync(id);

    public async Task AddAsync(Products product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Products product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Products product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
}