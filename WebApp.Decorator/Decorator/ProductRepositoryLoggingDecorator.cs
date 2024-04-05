using WebApp.Decorator.Models;
using WebApp.Decorator.Repositories;

namespace WebApp.Decorator.Decorator;

public class ProductRepositoryLoggingDecorator(IProductRepository productRepository,ILogger<ProductRepositoryLoggingDecorator> logger) :BaseProductRepositoryDecorator(productRepository)
{
    public override Task Update(Product product)
    {
        logger.LogInformation("Update metodu çalıştı");
        return base.Update(product);
    }

    public override Task<Product> Save(Product product)
    {
        logger.LogInformation("Save metodu çalıştı");
        return base.Save(product);
    }

    public override Task Remove(Product product)
    {
        logger.LogInformation("Remove metodu çalıştı");
        return base.Remove(product);
    }

    public override Task<List<Product>> GetAll()
    {
        logger.LogInformation("GetAll metodu çalıştı");
        return base.GetAll();
    }

    public override Task<List<Product>> GetAll(string userId)
    {
        logger.LogInformation("GetAll metodu çalıştı");
        return base.GetAll(userId);
    }

    public override Task<Product?> GetById(int id)
    {
        logger.LogInformation("GetById metodu çalıştı");
        return base.GetById(id);
    }
    
}