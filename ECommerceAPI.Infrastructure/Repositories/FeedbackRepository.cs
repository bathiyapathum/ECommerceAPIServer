using ECommerceAPI.Core.Entities.UserEntity;
using ECommerceAPI.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Repositories
{
    public class FeedbackRepository
    {
        private readonly ApplicationDbContext _context;

        public FeedbackRepository(ApplicationDbContext context)
        {
            //_users = database.GetCollection<User>("Users");
            _context = context;
        }

        public async Task CreateFeedbackAsync(Feedback feedback)
        {
            await _context._firestoreDb.Collection("Feedbacks").Document(feedback.Id = Guid.NewGuid().ToString()).SetAsync(feedback);
        }

    }
}
