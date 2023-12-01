using AutoMapper;
using ServiceManager.Core.Entities.Requests;
using ServiceManager.Core.Entities.Services;
using ServiceManager.UI.Models.Base;
using ServiceManager.UI.Models.Materials;
using ServiceManager.UI.Models.Requests;
using ServiceManager.UI.Models.Services;

namespace ServiceManager.UI.Config.Mapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<ServiceType, NamedModel>().ReverseMap();

            CreateMap<Material, MaterialModel>().ReverseMap();

            CreateMap<Service, ServiceModel>().ReverseMap();

            CreateMap<Request, RequestModel>().ReverseMap();

            CreateMap<RequestHistory, RequestExecuteModel>().ReverseMap();
        }
    }
}
