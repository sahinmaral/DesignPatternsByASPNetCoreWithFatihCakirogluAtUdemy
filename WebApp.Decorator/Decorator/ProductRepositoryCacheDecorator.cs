using Microsoft.Extensions.Caching.Memory;
using WebApp.Decorator.Models;
using WebApp.Decorator.Repositories;

namespace WebApp.Decorator.Decorator;

public class ProductRepositoryCacheDecorator(IProductRepository productRepository,IMemoryCache memoryCache) :BaseProductRepositoryDecorator(productRepository)
{
    private const string ProductsCacheName = "products";
    public override async Task<List<Product>> GetAll()
    {
        if (memoryCache.TryGetValue(ProductsCacheName, out List<Product> products))
        {
            return products;
        }

        await UpdateCache();
        return memoryCache.Get<List<Product>>(ProductsCacheName);
    }
    
    public override async Task<List<Product>> GetAll(string userId)
    {
        var products = await GetAll();
        return products.Where(product => product.UserId == userId).ToList();
    }

    public override async Task<Product> Save(Product product)
    {
        await base.Save(product);
        await UpdateCache();
        return product;
    }
    
    public override async Task Update(Product product)
    {
        await base.Update(product);
        await UpdateCache();
    }
    public override async Task Remove(Product product)
    {
        await base.Remove(product);
        await UpdateCache();
    }
    

    private async Task UpdateCache()
    {
        memoryCache.Set(ProductsCacheName, await base.GetAll());
    }
}