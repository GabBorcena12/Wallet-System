
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
            CreateMap<RegisterUserDto, User>().ReverseMap();
            CreateMap<TransactionDto, Transaction>().ReverseMap();
            CreateMap<TransactDto, Transaction>().ReverseMap();
            CreateMap<WithdrawDto, Transaction>().ReverseMap();
            CreateMap<DepositDto, Transaction>().ReverseMap();

        }
    }
}
