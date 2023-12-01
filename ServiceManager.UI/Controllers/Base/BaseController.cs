using Microsoft.AspNetCore.Mvc;
using ServiceManager.Core.Entities;
using ServiceManager.UI.Facades.Base;
using ServiceManager.UI.Models.Base;

namespace ServiceManager.UI.Controllers.Base
{
    public abstract class BaseController<TFacade, TFull, TSave, TTable, TEntity> : Controller
        where TFacade : BaseFacade<TFull, TSave, TTable, TEntity>
        where TFull : BaseModel, new()
        where TSave : BaseModel
        where TTable : BaseModel
        where TEntity : BaseEntity
    {
        protected TFacade _facade { get; set; } = null!;
        protected virtual string Title { get; } = null!;

        public BaseController(TFacade facade)
        {
            _facade = facade;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["ShowReportButton"] = this is IReportController;
            ViewData["Title"] = Title;
            return View("BaseIndex");
        }

        [HttpGet]
        public IActionResult Grid() =>
            PartialView("_GridPartial", _facade.Table().ToList());

        [HttpGet]
        public IActionResult Add()
        {
            FillViewDataForEdit();
            return PartialView("_ModalPartial", new TFull());
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            FillViewDataForEdit(id);
            return PartialView("_ModalPartial", _facade.GetById(id));
        }

        [HttpPost]
        public IActionResult Save(TSave model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Json(_facade.Save(model));
        }

        [HttpDelete]
        public void Delete(Guid id) => _facade.Delete(id);

        protected virtual void FillViewDataForEdit(Guid? id = null) { }
    }
}
