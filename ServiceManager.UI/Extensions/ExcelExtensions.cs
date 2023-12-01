using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ServiceManager.UI.Extensions
{
    public static class ExcelExtensions
    {
        public static void Border(this ExcelWorksheet worksheet,
            int startRow, int startCol, int endRow, int endCol, bool isBold = false)
        {
            var table = worksheet.Cells[startRow, startCol, endRow, endCol];

            table.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            table.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            table.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            table.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            table.Style.Border.BorderAround(ExcelBorderStyle.Medium);

            table.Style.Font.Bold = isBold;
        }

        public static void PrintRow(this ExcelWorksheet worksheet,
            int numRow, params object[] values)
        {
            int numCol = 1;
            foreach (var value in values)
                worksheet.Cells[numRow, numCol++].Value = value;
        }

        public static FileContentResult FileExcel(this Controller controller, ExcelPackage package, string fileName)
        {
            return controller.File(package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{fileName}_{DateTime.Now:yyyy.MM.dd_HH.mm}.xlsx");
        }
    }
}
