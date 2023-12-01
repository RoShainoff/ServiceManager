using ServiceManager.Core.Entities.Services;
using ServiceManager.UI.Controllers.Base;
using ServiceManager.UI.Facades.Services;
using ServiceManager.UI.Models.Base;
using ServiceManager.UI.Models.Services;

namespace ServiceManager.UI.Controllers.Services
{
    public class ServiceTypesController : BaseController<
        ServiceTypeFacade,
        NamedModel,
        NamedModel,
        ServiceTypeTableModel,
        ServiceType>
    {
        protected override string Title => "Очередь";

        public ServiceTypesController(ServiceTypeFacade facade)
            : base(facade) { }
    }
}
