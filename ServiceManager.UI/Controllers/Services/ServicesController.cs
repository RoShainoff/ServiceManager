using ServiceManager.Core.Entities.Services;
using ServiceManager.UI.Controllers.Base;
using ServiceManager.UI.Facades.Services;
using ServiceManager.UI.Models.Services;

namespace ServiceManager.UI.Controllers.Services
{
    public class ServicesController : BaseController<
        ServiceFacade,
        ServiceModel,
        ServiceModel,
        ServiceTableModel,
        Service>
    {
        protected override string Title => "Сервис";
        private readonly ServiceTypeFacade _serviceTypeFacade;

        public ServicesController(ServiceFacade facade, 
            ServiceTypeFacade serviceTypeFacade)
            : base(facade)
        {
            _serviceTypeFacade = serviceTypeFacade;
        }

        protected override void FillViewDataForEdit(Guid? id = null)
        {
            ViewData["ServiceTypes"] = _serviceTypeFacade.Lookup();
        }
    }
}
