using WebApp.Decorator.Models;
using WebApp.Decorator.Repositories;

namespace WebApp.Decorator.Decorator;

public class BaseProductRepositoryDecorator(IProductRepository productRepository):IProductRepository
{
    public virtual async Task<Product?> GetById(int id)
    {
        return await productRepository.GetById(id);
    }

    public virtual async Task<List<Product>> GetAll()
    {
        return await productRepository.GetAll();
    }

    public virtual async Task<List<Product>> GetAll(string userId)
    {
        return await productRepository.GetAll(userId);
    }

    public virtual async Task<Product> Save(Product product)
    {
        return await productRepository.Save(product);
    }

    public virtual async Task Update(Product product)
    {
        await productRepository.Update(product);
    }

    public virtual async Task Remove(Product product)
    {
        await productRepository.Remove(product);
    }
}