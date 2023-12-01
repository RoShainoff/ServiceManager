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
    public class ServiceTypeFacade : BaseFacade<
        NamedModel,
        NamedModel,
        ServiceTypeTableModel,
        ServiceType>
    {
        public ServiceTypeFacade(ServicesContext ctx, IMapper mapper)
            : base(ctx, mapper) { }

        public override IEnumerable<ServiceTypeTableModel> Table()
        {
            return _ctx.ServiceType
                .AsNoTracking()
                .Include(x => x.Services)
                .Select(x => new ServiceTypeTableModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ServiceCount = x.Services.Count,
                });
        }

        public IEnumerable<SelectListItem> Lookup()
        {
            return _ctx.ServiceType
                .AsNoTracking()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                });
        }
    }
}
