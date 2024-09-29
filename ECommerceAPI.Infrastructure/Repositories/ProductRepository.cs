using ECommerceAPI.Core.Entities;
using ECommerceAPI.Infrastructure.Persistance;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Repositories
{
    public class ProductRepository 
    {
        private readonly ApplicationDbContext _context;    

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task CreateAsync(Product products)
        {
            await _context.FirestoreDatabase.Collection("Products").Document(products.Id.ToString()).SetAsync(products);          
        }

        public async  Task<List<Product>> GetAllAsync()
        {
            var products = await _context.FirestoreDatabase.Collection("Products").GetSnapshotAsync();
            return products.Select(product => product.ConvertTo<Product>()).ToList();
        }
    }
}
