using AutoMapper;
using SupportPlatform.Database;

namespace SupportPlatform.Services
{
    public class ReportEntityMapper
    {
        private IMapper _mapper;

        public ReportEntityMapper()
        {
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<ReportDetailsToReturnDto, ReportEntity>().ReverseMap();
                config.CreateMap<AttachmentToReturnDto, AttachmentEntity>().ReverseMap();
                config.CreateMap<ResponseToReturnDto, ResponseEntity>().ReverseMap();
                config.CreateMap<ModificationEntryToReturnDto, ModificationEntryEntity>().ReverseMap();
            }).CreateMapper();
        }

        public ReportEntity Map(ReportDetailsToReturnDto reportDetailsToReturn) => _mapper.Map<ReportEntity>(reportDetailsToReturn);
        public ReportDetailsToReturnDto Map(ReportEntity reportEntity) => _mapper.Map<ReportDetailsToReturnDto>(reportEntity);
    }
}
