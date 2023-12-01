using Microsoft.AspNetCore.Mvc;
using ServiceManager.Core.Entities;
using ServiceManager.Core.Exceptions;
using ServiceManager.Core.Models.Identity;
using ServiceManager.UI.Controllers.Base;
using ServiceManager.UI.Facades.Services;
using ServiceManager.UI.Models.Identity;

namespace ServiceManager.UI.Controllers.Identity
{
    public class EmployeesController : BaseControllerAsync<
        EmployeeFacade,
        EmployeeModel,
        EmployeeModel,
        EmployeeTableModel,
        BaseEntity>
    {
        protected override string Title => "Агенты";

        public EmployeesController(EmployeeFacade facade) 
            : base(facade) { }

        public override async Task<IActionResult> Save(EmployeeModel model)
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

