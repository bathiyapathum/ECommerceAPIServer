using Amazon.Runtime.Internal.Transform;
using ECommerceAPI.Core.Entities.UserEntity;
using ECommerceAPI.Infrastructure.Persistance;
using FirebaseAdmin.Auth.Hash;
using Google.Cloud.Firestore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Repositories
{
    public class UserRepository
    {
        //private readonly IMongoCollection<User> _users;
        private readonly ApplicationDbContext _context;
        //private readonly FirestoreDatabase _firestoreDb;

        public UserRepository(ApplicationDbContext context)
        {
            //_users = database.GetCollection<User>("Users");
            _context = context;
        }

        public FirestoreDb FirestoreDatabase => _context._firestoreDb;

        public async Task<User> GetUserbyEmailAsync(string email)
        {
            //return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
            return await _context._firestoreDb.Collection("Users")
                .WhereEqualTo("email",email)
                .Limit(1)
                .GetSnapshotAsync()
                .ContinueWith(task =>
                {
                    var snapshot = task.Result;
                    if (snapshot.Count == 0)
                    {
                        return null;
                    }
                    return snapshot.Documents[0].ConvertTo<User>();
                });
        }


        public async Task CreateUserAsync(User user)
        {
            //await _users.InsertOneAsync(user);

            await _context._firestoreDb.Collection("Users").Document(user.Id= Guid.NewGuid().ToString()).SetAsync(user);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var documentSnapShot = await _context._firestoreDb.Collection("Users")
                .Document(id)
                .GetSnapshotAsync();

            if(!documentSnapShot.Exists)
            {
                return null;
            }

            return documentSnapShot.ConvertTo<User>();
        }

        public async Task<bool> UpdateUserAsync(User userforChangePassword, ChangePassword changePasswordReqDTO)
        {
            try
            {
                await _context._firestoreDb.Collection("Users")
                .Document(userforChangePassword.Id)
                .UpdateAsync(new Dictionary<string, object>
                {
                    { "passwordHash", changePasswordReqDTO.NewPassword},
                    { "updateTime", DateTime.UtcNow},
                    { "isActive", true }
                });
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }   

        }

        public async Task<bool> UpdateUserAsync(User userforUpdate,string userID)
        {
            try
            {
                await _context._firestoreDb.Collection("Users")
                    .Document(userID)
                    .UpdateAsync(new Dictionary<string, object>
                    {
                        //{ "isActive", true },
                        //{ "updateTime", DateTime.UtcNow }
                        { "firstName" ,userforUpdate.FirstName},
                        { "lastName", userforUpdate.LastName },
                        { "phoneNumber", userforUpdate.PhoneNumber },
                        { "addresss.city", userforUpdate.Addresss.City },
                        {"addresss.country", userforUpdate.Addresss.Country },
                        { "addresss.state", userforUpdate.Addresss.State },
                        { "addresss.street", userforUpdate.Addresss.Street },
                        { "addresss.zipCode", userforUpdate.Addresss.ZipCode },
                        { "updateTime", DateTime.UtcNow },
                        { "profilePicture", userforUpdate.ProfilePicture }
                    });

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ActivateCustomer(User userforActivation)
        {
            try
            {
                await _context._firestoreDb
                    .Collection("Users")
                    .Document(userforActivation.Id)
                    .UpdateAsync(new Dictionary<string, object>
                    {
                        { "isActive", true },
                        { "updateTime", DateTime.UtcNow }
                    });

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeactivateUser(User userforDeactivate)
        {
            try
            {
                await _context._firestoreDb.Collection("Users")
                    .Document(userforDeactivate.Id)
                    .UpdateAsync(new Dictionary<string, object>
                    {
                        { "isActive", false },
                        { "updateTime", DateTime.UtcNow }
                    });
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<User>> GetInactiveUsers()
        {
            try
            {
                var snapShot = await _context._firestoreDb.Collection("Users")
                    //.WhereEqualTo("isActive", false)
                    .GetSnapshotAsync();

                if(snapShot.Count == 0)
                {
                    return null;
                }

                return snapShot.Documents.Select(doc => doc.ConvertTo<User>()).ToList();
                    //.ContinueWith(task =>
                    //{
                    //    var snapshot = task.Result;
                    //    if (snapshot.Count == 0)
                    //    {
                    //        return null;
                    //    }
                    //    return snapshot.Documents.Select(doc => doc.ConvertTo<User>()).ToList();
                    //});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public async Task<Feedback> CreateFeedbackAsync(Feedback feedback, Transaction transaction)
        //{
        //    //await _context._firestoreDb.Collection("Feedbacks").Document(feedback.Id = Guid.NewGuid().ToString()).SetAsync(feedback);
        //    //return feedback;
        //    feedback.Id = Guid.NewGuid().ToString();
        //    var feedbackRef = _context._firestoreDb.Collection("Feedbacks").Document(feedback.Id);
        //    transaction.Set(feedbackRef, feedback);

        //    return feedback;

        //}
    }
}
