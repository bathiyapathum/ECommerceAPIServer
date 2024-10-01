using ECommerceAPI.Application.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces
{
    public interface IValidations
    {
        public List<string> ValidateUserInputs(SignupReqDTO signupReqDTO);
        public List<string> ValidateUserRole(SignupReqDTO signupReqDTO, UserRole role);
        public bool IsValidEmail(string email);
    }
}
