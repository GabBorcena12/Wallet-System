
using ExamWalletSystem.Model;
using AutoMapper;
using ExamWalletSystem.Model.Dto;

namespace ExamWalletSystem.Configurations
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<UserDto, User>().ReverseMap(); 

        }
    }
}
