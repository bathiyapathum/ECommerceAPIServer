using ECommerceAPI.Application.DTOs;
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
        public bool IsValidEmail(string email);
    }
}
