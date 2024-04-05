using Microsoft.EntityFrameworkCore;
using WebApp.Decorator.Models;

namespace WebApp.Decorator.Repositories;

public class ProductRepository(AppIdentityDbContext dbContext) : IProductRepository
{
    public async Task<Product?> GetById(int id)
    {
        return await dbContext.Products.FindAsync(id);
    }

    public async Task<List<Product>> GetAll()
    {
        return await dbContext.Products.ToListAsync();
    }

    public async Task<List<Product>> GetAll(string userId)
    {
        return await dbContext.Products.Where(product => product.UserId == userId).ToListAsync();
    }

    public async Task<Product> Save(Product product)
    {
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();
        return product;
    }

    public async Task Update(Product product)
    {
        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync();
    }

    public async Task Remove(Product product)
    {
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
    }
}