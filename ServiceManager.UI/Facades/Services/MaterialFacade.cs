using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Core;
using ServiceManager.Core.Entities.Services;
using ServiceManager.UI.Facades.Base;
using ServiceManager.UI.Models.Materials;
using ServiceManager.UI.Models.Materials.Report;

namespace ServiceManager.UI.Facades.Services
{
    public class MaterialFacade : BaseFacade<
        MaterialModel,
        MaterialModel,
        MaterialTableModel,
        Material>
    {
        public MaterialFacade(ServicesContext ctx, IMapper mapper)
            : base(ctx, mapper) { }

        public override IEnumerable<MaterialTableModel> Table()
        {
            return _ctx.Material
                .AsNoTracking()
                .Include(x => x.Requests)
                .Select(x => new MaterialTableModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Count = x.Count,
                    RequestsCount = x.Requests.Count(),
                });
        }

        public IEnumerable<SelectListItem> Lookup()
        {
            return _ctx.Material
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                });
        }

        public IEnumerable<Material> GetAll()
        {
            return _dbSet.OrderBy(x => x.Name);
        }

        public IEnumerable<MaterialForReport> GetForReport()
        {
            return _ctx.RequestMaterial
                .Select(x => new MaterialForReport
                {
                    RequestName = $"#{x.Request.Number}",
                    MaterialName = x.Material.Name,
                    MaterialCount = x.Count,
                });
        }
    }
}
