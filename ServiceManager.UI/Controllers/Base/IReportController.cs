using Microsoft.AspNetCore.Mvc;

namespace ServiceManager.UI.Controllers.Base
{
    public interface IReportController
    {
        [HttpGet]
        FileResult Report();
    }
}
