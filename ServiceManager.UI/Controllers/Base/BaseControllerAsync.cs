using Microsoft.AspNetCore.Mvc;
using ServiceManager.Core.Entities;
using ServiceManager.UI.Facades.Base;

namespace ServiceManager.UI.Controllers.Base
{
    public abstract class BaseControllerAsync<TFacade, TFull, TSave, TTable, TEntity> : Controller
        where TFacade : BaseFacadeAsync<TFull, TSave, TTable, TEntity>
        where TFull : class, new()
        where TSave : class
        where TTable : class
        where TEntity : BaseEntity
    {
        protected TFacade _facade { get; set; } = null!;
        protected virtual string Title { get; } = null!;

        public BaseControllerAsync(TFacade facade)
        {
            _facade = facade;
        }

        [HttpGet]
        public virtual IActionResult Index()
        {
            ViewData["ShowReportButton"] = this is IReportController;
            ViewData["Title"] = Title;
            return View("BaseIndex");
        }

        [HttpGet]
        public virtual IActionResult Grid() =>
            PartialView("_GridPartial", _facade.Table().ToList());

        [HttpGet]
        public virtual async Task<IActionResult> Add()
        {
            await FillViewDataForEdit();
            return PartialView("_ModalPartial", new TFull());
        }

        [HttpGet]
        public virtual async Task<IActionResult> Edit(Guid id, bool edit = true)
        {
            await FillViewDataForEdit(id);
            var model = await _facade.GetById(id);
            ViewData["Edit"] = edit;
            return PartialView("_ModalPartial", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Save(TSave model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            
            await _facade.Save(model);

            return Ok();
        }

        [HttpDelete]
        public virtual async Task Delete(Guid id) => await _facade.Delete(id);

        protected virtual async Task FillViewDataForEdit(Guid? id = null) { }
    }

}
