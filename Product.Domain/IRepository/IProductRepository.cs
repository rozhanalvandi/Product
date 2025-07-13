using  Product.Domain.Models; 
namespace Product.Domain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Products>> GetAllAsync(string? createdBy = null);
    Task<Products?> GetByIdAsync(int id);
    Task AddAsync(Products product);
    Task UpdateAsync(Products product);
    Task DeleteAsync(Products product);
}
