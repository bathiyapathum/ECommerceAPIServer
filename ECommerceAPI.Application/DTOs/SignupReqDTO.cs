using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs
{
    public class SignupReqDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Re_Password { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public Address Addresss { get; set; }

        private UserRole _role;
        public UserRole Role
        {
            get => _role;
            set => _role = IsValidUserRole(value) ? value : UserRole.Customer;
        }
        //public string Status { get; set; } = string.Empty;
        //public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        //public string UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = false;
        //public bool IsDeleted { get; set; } = false;
        public string ProfilePicture { get; set; } = string.Empty;

        //public string HashedPassword { get; set; }
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
        Vendor = 4
    }

    public class Address
    {

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}
