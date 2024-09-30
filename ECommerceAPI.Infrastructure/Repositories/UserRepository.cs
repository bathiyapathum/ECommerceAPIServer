using ECommerceAPI.Core.Entities;
using ECommerceAPI.Infrastructure.Persistance;
using FirebaseAdmin.Auth.Hash;
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

        public UserRepository(ApplicationDbContext context)
        {
            //_users = database.GetCollection<User>("Users");
            _context = context;
        }

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
                    {"isActive", true }
                });
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }   

        }
    }
}
