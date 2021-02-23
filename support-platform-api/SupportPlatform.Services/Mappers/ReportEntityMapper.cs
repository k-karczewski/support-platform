using AutoMapper;
using SupportPlatform.Database;
using System.Collections.Generic;

namespace SupportPlatform.Services
{
    public class ReportEntityMapper
    {
        private IMapper _mapper;

        public ReportEntityMapper()
        {
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<ReportDetailsToReturnDto, ReportEntity>().ReverseMap()
                            .ForMember(dest => dest.Date, opt => opt.MapFrom(y => y.Date.ToString("dd/MM/yyyy HH:mm:ss")))
                            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(y => y.User.UserName));

                config.CreateMap<AttachmentToReturnDto, AttachmentEntity>().ReverseMap();
                config.CreateMap<ResponseToReturnDto, ResponseEntity>().ReverseMap().ForMember(dest => dest.Date, opt => opt.MapFrom(y => y.Date.ToString("dd/MM/yyyy HH:mm:ss")));
                config.CreateMap<ModificationEntryToReturnDto, ModificationEntryEntity>().ReverseMap().ForMember(dest => dest.Date, opt => opt.MapFrom(y => y.Date.ToString("dd/MM/yyyy HH:mm:ss")));


                config.CreateMap<ReportListItemToReturnDto, ReportEntity>().ReverseMap()
                            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(y => y.User.UserName))
                            .ForMember(dest => dest.Date, opt => opt.MapFrom(y => y.Date.ToString("dd/MM/yyyy")))
                            .ForMember(dest => dest.Status, opt => opt.MapFrom(y => (int)y.Status));

            }).CreateMapper();
        }

        public ReportEntity Map(ReportDetailsToReturnDto reportDetailsToReturn) => _mapper.Map<ReportEntity>(reportDetailsToReturn);
        public ReportDetailsToReturnDto Map(ReportEntity reportEntity) => _mapper.Map<ReportDetailsToReturnDto>(reportEntity);

        public ICollection<ReportListItemToReturnDto> Map(ICollection<ReportEntity> reportEntities) => _mapper.Map<ICollection<ReportListItemToReturnDto>>(reportEntities);


    }
}
