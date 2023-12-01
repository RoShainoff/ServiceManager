using Microsoft.AspNetCore.Mvc;
using ServiceManager.Core.Entities;
using ServiceManager.Core.Exceptions;
using ServiceManager.Core.Models.Identity;
using ServiceManager.UI.Controllers.Base;
using ServiceManager.UI.Facades.Services;
using ServiceManager.UI.Models.Identity;

namespace ServiceManager.UI.Controllers.Identity
{
    public class ClientsController : BaseControllerAsync<
        ClientFacade,
        ClientModel,
        ClientModel,
        ClientTableModel,
        BaseEntity>
    {
        protected override string Title => "Клиенты";

        public ClientsController(ClientFacade facade) 
            : base(facade) { }

        public override async Task<IActionResult> Save(ClientModel model)
        {
            try
            {
                await _facade.Save(model);
                return Ok();
            }
            catch (IdentityException ex)
            {
                foreach (var item in ex.Errors)
                    ModelState.AddModelError(string.Empty, item.Description);
                return BadRequest(ModelState);
            }
        }
    }
}

