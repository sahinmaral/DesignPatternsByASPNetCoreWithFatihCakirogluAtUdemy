using MongoDB.Driver;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Repositories;

public class ProductRepositoryFromMongoDb : IProductRepository
{
    private readonly IMongoCollection<Product> _productCollection;

    public ProductRepositoryFromMongoDb(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDbConnectionString");
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("introDesignPatternsDb");
        _productCollection = database.GetCollection<Product>("products");
    }

    public async Task<Product?> GetById(string id)
    {
        return await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public Task<List<Product>> GetAllByUserId(string userId)
    {
        return _productCollection.Find(p => p.UserId == userId).ToListAsync();
    }

    public async Task<Product> Save(Product product)
    {
        product.Id = Guid.NewGuid().ToString("N").Substring(0, 24);
        await _productCollection.InsertOneAsync(product);
        return product;
    }

    public async Task Update(Product product)
    {
        await _productCollection.FindOneAndReplaceAsync(p => p.Id == product.Id, product);
    }

    public async Task Delete(Product product)
    {
        await _productCollection.DeleteOneAsync(p => p.Id == product.Id);
    }
}