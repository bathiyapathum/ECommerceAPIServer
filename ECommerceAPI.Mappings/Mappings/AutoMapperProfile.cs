using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Core.Entities;

namespace ECommerceAPI.Mappings.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SignupReqDTO, User>().ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
