using Microsoft.EntityFrameworkCore;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Repositories;

public class ProductRepositoryFromSqlServer(AppIdentityDbContext dbContext) : IProductRepository
{
    public async Task<Product?> GetById(string id)
    {
        return await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Product>> GetAllByUserId(string userId)
    {
        return await dbContext.Products.Where(p => p.UserId == userId).ToListAsync();
    }

    public async Task<Product> Save(Product product)
    {
        product.Id = Guid.NewGuid().ToString();
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        return product;
    }

    public async Task Update(Product product)
    {
        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(Product product)
    {
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
    }
}