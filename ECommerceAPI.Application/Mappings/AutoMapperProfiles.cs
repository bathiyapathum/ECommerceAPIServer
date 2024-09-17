using AutoMapper;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Core.Entities;


namespace ECommerceAPI.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SignupReqDTO, User>().ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
