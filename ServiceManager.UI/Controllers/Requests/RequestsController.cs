using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceManager.Core.Entities.Requests;
using ServiceManager.Core.Enums;
using ServiceManager.Core.Repositories;
using ServiceManager.UI.Controllers.Base;
using ServiceManager.UI.Extensions;
using ServiceManager.UI.Facades.Requests;
using ServiceManager.UI.Facades.Services;
using ServiceManager.UI.Models.Requests;
using System.Diagnostics;
using System.Security.Claims;

namespace ServiceManager.UI.Controllers.Requests
{
    [Authorize]
    public class RequestsController : BaseControllerAsync<
        RequestFacade,
        RequestModel,
        RequestModel,
        RequestTableModel,
        Request>, IReportController
    {
        protected override string Title => "Заявки";

        private readonly ClientFacade _clientFacade;
        private readonly EmployeeFacade _employeeFacade;
        private readonly ServiceTypeFacade _serviceTypeFacade;
        private readonly ServiceFacade _serviceFacade;
        private readonly MaterialFacade _materialFacade;
        private readonly IdentityRepository _identityRepository;

        public RequestsController(RequestFacade facade,
            ClientFacade clientFacade,
            EmployeeFacade employeeFacade,
            ServiceTypeFacade serviceTypeFacade,
            ServiceFacade serviceFacade,
            MaterialFacade materialFacade,
            IdentityRepository identityRepository) : base(facade)
        {
            _clientFacade = clientFacade;
            _employeeFacade = employeeFacade;
            _serviceTypeFacade = serviceTypeFacade;
            _serviceFacade = serviceFacade;
            _materialFacade = materialFacade;
            _identityRepository = identityRepository;
        }

        [HttpGet]
        public override async Task<IActionResult> Add()
        {
            await FillViewDataForEdit();
            var model = new RequestModel();

            var userId = _identityRepository.GetUserId();
            if (User.IsEmployeeOrAdmin())
                model.ExecutorId = userId;
            else
                model.ClientId = userId;
            ViewData["Edit"] = true;
            return PartialView("_ModalPartial", model);
        }

        [HttpGet]
        public IActionResult GridWithFilter(RequestFilter filter)
        {
            var userId = _identityRepository.GetUserId();
            IEnumerable<RequestTableModel> items = _facade.Table();

            if (User.IsInRole("Admin"))
                items = items;
            else if (User.IsInRole("Employee"))
                items = items.Where(x => x.Status == RequestAction.Create || x.ExecuterId == userId);
            else
                items = items.Where(x => x.ClientId == userId);
            items = items.ToList();

            if (filter == RequestFilter.All)
                items = items.Where(x => !x.IsClosed);
            if (filter == RequestFilter.My)
                items = items.Where(x => !x.IsClosed)
                    .Where(x => x.ExecuterId == userId || x.ClientId == userId);
            else if (filter == RequestFilter.Complited)
                items = items.Where(x => x.IsClosed);

            return PartialView("_GridPartial", items.ToList());
        }


        protected override Task FillViewDataForEdit(Guid? id = null)
        {
            ViewData["Clients"] = _clientFacade.Lookup();
            ViewData["Executors"] = _employeeFacade.Lookup();
            ViewData["ServiceTypes"] = _serviceTypeFacade.Lookup();
            ViewData["Services"] = _serviceFacade.Lookup();

            return Task.CompletedTask;
        }

        [HttpGet]
        public async Task<IActionResult> Execute([FromQuery] Guid requestId, [FromQuery] RequestAction action)
        {
            var currentTransactions = await _facade.GetNextTransactions(requestId);

            if (!currentTransactions.Contains(action))
                throw new ArgumentException("Не корректный переход", nameof(action));

            var request = await _facade.GetById(requestId);
            var model = new TransitionModel
            {
                RequestId = requestId,
                Action = action,
                Number = request.Number.GetValueOrDefault(),
                Text = request.Text,         
                ServiceId = request.ServiceId,
                ServiceTypeId = request.ServiceTypeId,
            };

            ViewData["ServiceTypes"] = _serviceTypeFacade.Lookup();
            ViewData["Services"] = _serviceFacade.Lookup();
            ViewData["Materials"] = _materialFacade.GetAll().ToList();

            return PartialView("_ExecuteForm", model);
        }

        [HttpPost]
        public async Task Execute(TransitionModel model)
        {
            var request = await _facade.GetById(model.RequestId);

            if (model.Action == RequestAction.Accept)
            {
                if (model.ServiceTypeId == null)
                    throw new ArgumentNullException(nameof(model.ServiceTypeId));
                if (model.ServiceId == null) 
                    throw new ArgumentNullException(nameof(model.ServiceId));

                request.ExecutorId = _identityRepository.GetUserId();
                request.ServiceId = model.ServiceId;
                request.ServiceTypeId = model.ServiceTypeId;

                await _facade.Save(request, notifyAfterSave: false);
            }
            var historyId = await _facade.Execute(new RequestExecuteModel
            {
                UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                Action = model.Action,
                Note = model.Note,
                RequestId = model.RequestId,
            });
            await _facade.AddMaterials(model.RequestId, model.Materials);
            await _facade.Notify(historyId, request.ClientId);
        }

        [HttpGet]
        public IActionResult MaterialsRowPartial()
        {
            ViewData["Materials"] = _materialFacade.GetAll().ToList();
            return PartialView("_MaterialRow", new RequestMaterialModel());
        }

        public FileResult Report()
        {
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Отчёт")!;
            worksheet.OutLineSummaryBelow = false;
            worksheet.OutLineSummaryRight = false;

            var agents = _facade.GetForReport().ToList();
            int numRow = 1;
            int startCol = 1;
            int colCount = 5;

            worksheet.Border(numRow, startCol, numRow, colCount, isBold: true);
            worksheet.PrintRow(numRow++,
                "Агент/Очередь",
                "Кол-во заявок",
                "Завершённые",
                "Удачно",
                "Неудачно");

            if (!agents.Any())
                return this.FileExcel(package, $"Отчёт");

            foreach (var agent in agents)
            {
                worksheet.Border(numRow, startCol, numRow, colCount);
                worksheet.Cells[numRow, startCol].Style.Font.Bold = true;
                worksheet.PrintRow(numRow++,
                    agent.FullName,
                    agent.CountAll,
                    agent.CountComplited,
                    agent.CountGoodComplited,
                    agent.CountBadComplited);

                int startSubTable = numRow;
                foreach (var serviceType in agent.ServiceTypes)
                {
                    worksheet.Row(numRow).OutlineLevel = startCol;
                    worksheet.PrintRow(numRow++,
                        serviceType.Name,
                        serviceType.CountAll,
                        serviceType.CountComplited,
                        serviceType.CountGoodComplited,
                        serviceType.CountBadComplited);
                }
                worksheet.Border(startSubTable, startCol, numRow - 1, colCount);
            }
            worksheet.Cells.AutoFitColumns();
            return this.FileExcel(package, $"Отчёт");
        }
    }
}
