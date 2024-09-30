using ECommerceAPI.Core.Entities.OrderEntity;
using ECommerceAPI.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Repositories
{
    public class OrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Order order)
        {
            await _context.FirestoreDatabase.Collection("Orders").Document(order.OrderId.ToString()).SetAsync(order);
        }

    }
}
