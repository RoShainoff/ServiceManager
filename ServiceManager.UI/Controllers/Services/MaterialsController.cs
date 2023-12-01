using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using ServiceManager.Core.Entities.Services;
using ServiceManager.UI.Controllers.Base;
using ServiceManager.UI.Extensions;
using ServiceManager.UI.Facades.Services;
using ServiceManager.UI.Models.Materials;

namespace ServiceManager.UI.Controllers.Services
{
    public class MaterialsController : BaseController<
        MaterialFacade,
        MaterialModel,
        MaterialModel,
        MaterialTableModel,
        Material>, IReportController
    {
        protected override string Title => "Материалы";

        public MaterialsController(MaterialFacade facade) : base(facade) { }

        public FileResult Report()
        {
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Отчёт")!;
            worksheet.OutLineSummaryBelow = false;
            worksheet.OutLineSummaryRight = false;

            var requests = _facade.GetForReport().ToList().GroupBy(x => x.RequestName);
            int numRow = 1;
            int startCol = 1;
            int colCount = 2;

            worksheet.Border(numRow, startCol, numRow, colCount, isBold: true);
            worksheet.PrintRow(numRow++,
                "Заявка/Материал",
                "Кол-во");

            if (!requests.Any())
                return this.FileExcel(package, $"Отчёт");

            foreach (var request in requests)
            {
                worksheet.Border(numRow, startCol, numRow, colCount);
                worksheet.Cells[numRow, startCol].Style.Font.Bold = true;
                worksheet.PrintRow(numRow++,
                    request.Key,
                    request.Sum(x => x.MaterialCount));

                int startSubTable = numRow;
                foreach (var material in request)
                {
                    worksheet.Row(numRow).OutlineLevel = startCol;
                    worksheet.PrintRow(numRow++,
                        material.MaterialName,
                        material.MaterialCount);
                }
                worksheet.Border(startSubTable, startCol, numRow - 1, colCount);
            }
            worksheet.Cells.AutoFitColumns();
            return this.FileExcel(package, $"Отчёт");
        }
    }
}
