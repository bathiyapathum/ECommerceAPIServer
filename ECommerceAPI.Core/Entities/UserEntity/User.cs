using ECommerceAPI.Core.Entities.ProductEntity;
using Google.Cloud.Firestore;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Core.Entities.UserEntity
{
    [FirestoreData]
    public class User
    {
        //[BsonId]
        //[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        //public string Id { get; set; }
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty("email")]
        public string Email { get; set; }

        [FirestoreProperty("firstName")]
        public string FirstName { get; set; }

        [FirestoreProperty("lastName")]
        public string LastName { get; set; }

        [FirestoreProperty("passwordHash")]
        public string PasswordHash { get; set; }
        public string Re_PasswordHash { get; set; }

        private UserRole _role;
        [FirestoreProperty("role")]
        public UserRole Role
        {
            get => _role;
            set => _role = IsValidUserRole(value) ? value : UserRole.Customer;
        }



        [FirestoreProperty("createdOn")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [FirestoreProperty("updatedOn")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        [FirestoreProperty("isActive")]
        public bool IsActive { get; set; } = false;

        [FirestoreProperty("emailConfirmed")]
        public bool EmailConfirmed { get; set; } = false;

        [FirestoreProperty("lastLogin")]
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;

        [FirestoreProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        [FirestoreProperty("address")]
        public Address Addresss { get; set; }

        [FirestoreProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [FirestoreProperty("feedbackInfo")]
        public VendorFeedbackInfo FeedbackInfo { get; set; }

        public string Password { get; set; }

        private bool IsValidUserRole(UserRole role)
        {
            return Enum.IsDefined(typeof(UserRole), role);
        }


    }

    public enum UserRole
    {
        Admin = 1,
        Customer = 2,
        CSR = 3,
        vendor = 4
    }

    [FirestoreData]
    public class Address
    {
        [FirestoreProperty("street")]
        public string Street { get; set; }

        [FirestoreProperty("city")]
        public string City { get; set; }

        [FirestoreProperty("state")]
        public string State { get; set; }

        [FirestoreProperty("country")]
        public string Country { get; set; }

        [FirestoreProperty("zipCode")]
        public string ZipCode { get; set; }
    }

    [FirestoreData]
    public class VendorFeedbackInfo
    {
        [FirestoreProperty("feedbackCount")]
        public int FeedbackCount { get; set; }

        [FirestoreProperty("sumOfRating")]
        public int SumOfRating { get; set; }
    }
}
