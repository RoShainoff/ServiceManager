using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceManager.UI.Facades.Requests;

namespace ServiceManager.UI.Controllers.Requests
{
    [Authorize]
    public class RequestsNotifyController : Controller
    {
        private readonly RequestNotifyFacade _facede;

        public RequestsNotifyController(RequestNotifyFacade facede)
        {
            _facede = facede;
        }

        [HttpGet]
        public JsonResult GetCount()
        {
            var count = _facede.GetCountUnreadByUser();
            return Json(count);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _facede.GetAllByUser().ToList();
            return PartialView("_Notifies", result);
        }

        [HttpPost]
        public IActionResult Read(Guid notifyId)
        {
            _facede.Read(notifyId);
            return Ok();
        }
    }
}
