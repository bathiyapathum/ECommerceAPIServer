using ECommerceAPI.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Persistance
{
    public class ProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(IMongoDatabase database)
        {
            _products = database.GetCollection<Product>("Products");
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _products.Find(p => true).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task UpdateAsync(Guid id, Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == id, product);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _products.DeleteOneAsync(p => p.Id == id);
        }
    }
}
