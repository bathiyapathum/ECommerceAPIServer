using AutoMapper;
using ECommerceAPI.Application.DTOs.UserDTO;
using ECommerceAPI.Core.Entities.UserEntity;


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
