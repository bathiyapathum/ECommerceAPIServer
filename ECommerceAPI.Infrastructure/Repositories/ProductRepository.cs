using ECommerceAPI.Core.Entities;
using ECommerceAPI.Infrastructure.Persistance;
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
    }
}
