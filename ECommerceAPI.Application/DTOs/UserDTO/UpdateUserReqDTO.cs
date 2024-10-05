using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.UserDTO
{
    public class UpdateUserReqDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public Address Addresss { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public string ProfilePicture { get; set; } = string.Empty;

        //public string HashedPassword { get; set; }
        private bool IsValidUserRole(UserRole role)
        {
            return Enum.IsDefined(typeof(UserRole), role);
        }

    }
}
