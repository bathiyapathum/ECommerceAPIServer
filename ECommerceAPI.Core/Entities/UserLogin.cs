using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Core.Entities
{
    [FirestoreData]
    public class UserLogin
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty("email")]
        public string Email { get; set; }

        [FirestoreProperty("firstName")]
        public string FirstName { get; set; }

        [FirestoreProperty("lastName")]
        public string LastName { get; set; }

        //[FirestoreProperty("passwordHash")]
        //public string PasswordHash { get; set; }
        //public string Re_PasswordHash { get; set; }

        [FirestoreProperty("role")]
        public UserRoleLogin Role { get; set; } = UserRoleLogin.Customer;

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
        public AddressLogin Addresss { get; set; }

        [FirestoreProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

    }

    public enum UserRoleLogin
    {
        Admin = 1,
        Customer = 2,
        CSR = 3
    }

    [FirestoreData]
    public class AddressLogin
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

}
