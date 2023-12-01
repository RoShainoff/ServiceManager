using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Core;
using ServiceManager.Core.Entities.Services;
using ServiceManager.UI.Facades.Base;
using ServiceManager.UI.Models.Base;
using ServiceManager.UI.Models.Services;

namespace ServiceManager.UI.Facades.Services
{
    public class ServiceFacade : BaseFacade<
        ServiceModel,
        ServiceModel,
        ServiceTableModel,
        Service>
    {
        public ServiceFacade(ServicesContext ctx, IMapper mapper)
            : base(ctx, mapper) { }

        public override IEnumerable<ServiceTableModel> Table()
        {
            return _ctx.Service
                .AsNoTracking()
                .Include(x => x.Requests)
                .Select(x => new ServiceTableModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Cost = x.Cost,
                    Hours = x.Hours,
                    RequestCount = x.Requests.Count,
                    ServiceType = x.ServiceType == null ? null :
                        new NamedModel
                        {
                            Id = x.ServiceType.Id,
                            Name = x.ServiceType.Name,
                        }
                });
        }

        public IEnumerable<SelectListItem> Lookup(Guid? serviceTypeId = null)
        {
            return _ctx.Service
                .Where(x => x.ServiceType != null)
                .Where(x => serviceTypeId == null || x.ServiceTypeId == serviceTypeId)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,

                    Group = new SelectListGroup
                    {
                        Name = x.ServiceType!.Name
                    }
                });
        }
    }
}
