using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs
{
    public class SignupReqDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        //public string HashedPassword { get; set; }

    }
}
