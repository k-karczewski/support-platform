using AutoMapper;
using SupportPlatform.Database;

namespace SupportPlatform.Services
{
    public class UserEntityMapper
    {
        private IMapper _mapper;

        public UserEntityMapper()
        {
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<UserToRegisterDto, UserEntity>().ReverseMap();
            }).CreateMapper();
        }

        public UserEntity Map(UserToRegisterDto userToRegisterDto) => _mapper.Map<UserEntity>(userToRegisterDto);
        public UserToRegisterDto Map(UserEntity userEntity) => _mapper.Map<UserToRegisterDto>(userEntity);
    }
}
