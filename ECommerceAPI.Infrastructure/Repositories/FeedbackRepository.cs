using ECommerceAPI.Core.Entities.OrderEntity;
using ECommerceAPI.Core.Entities.ProductEntity;
using ECommerceAPI.Core.Entities.UserEntity;
using ECommerceAPI.Infrastructure.Persistance;
using Google.Cloud.Firestore;
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

        public FirestoreDb FirestoreDatabase => _context._firestoreDb;

        public async Task CreateFeedbackAsync(Feedback feedback)
        {
            await _context._firestoreDb.Collection("Feedbacks").Document(feedback.Id = Guid.NewGuid().ToString()).SetAsync(feedback);
        }

        public async Task<bool> GetFeedbackByIdAsync(string feedbackId)
        {
            //await _context._firestoreDb.Collection("Feedbacks").Document(feedbackId).GetSnapshotAsync();

            try
            {
                var result = await _context.FirestoreDatabase.Collection("Feedbacks").Document(feedbackId).GetSnapshotAsync();
                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while execution of CreateOrderItemAsync:  {ex.Message}");

            }
        }

        public async Task<Feedback> CreateFeedbackAsync(Feedback feedback, Transaction transaction)
        {
            //await _context._firestoreDb.Collection("Feedbacks").Document(feedback.Id = Guid.NewGuid().ToString()).SetAsync(feedback);
            //return feedback;

            try
            {
                feedback.Id = Guid.NewGuid().ToString();
                var feedbackRef = _context._firestoreDb.Collection("Feedbacks").Document(feedback.Id);
                transaction.Set(feedbackRef, feedback);

                return feedback;
            }

            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating feedback.", ex); 
            }

        }

        public async Task<bool> GetFeedbackByCustomerAndProductAsync(string customerId, string productId, string orderId)
        {
            try
            {
                DocumentReference productRef = _context._firestoreDb.Collection("VendorProducts").Document(productId);

                DocumentSnapshot productSnapshot = await productRef.GetSnapshotAsync();

                if (productSnapshot.Exists)
                {
                    VendorProduct product = productSnapshot.ConvertTo<VendorProduct>();

                    if(product.FeedbackInfo == null)
                    {
                        return false;
                    }

                    foreach (var feedBack in product.FeedbackInfo)
                    {
                        if (feedBack.CustomerId == customerId && feedBack.OrderId == orderId)
                        {
                            return true;
                        }
                        return false;
                    }
                }

                return false;
            }
            catch(Exception ex)
            {
                throw new Exception();
            }
        }


    }
}
