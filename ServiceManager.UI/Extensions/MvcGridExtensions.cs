using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NonFactors.Mvc.Grid;
using ServiceManager.UI.Models.Base;

namespace ServiceManager.UI.Extensions
{
    public static class MvcGridExtensions
    {
        public static IHtmlGrid<T> BaseGrid<T>(this IHtmlHelper<IEnumerable<T>> html, IEnumerable<T> source,
            Action<IGridColumnsOf<T>> builder, bool addNumber = true) where T : BaseTableModel, new()
        {
            var items = source ?? Enumerable.Empty<T>();

            var grid = html.Grid(items)
                .Id("mvc-grid");
            if (addNumber)
                grid.Build(AddNumberCollumn);
            grid = grid
                .Build(builder)
                .Sortable()
                .Css("table table-borderless table-hover table-striped")
                .Filterable()
                .Pageable(pager =>
                {
                    pager.PageSizes = new Dictionary<int, string> { { 10, "10" }, { int.MaxValue, "Все" }, };
                    pager.ShowPageSizes = true;
                    pager.RowsPerPage = 10;
                });

            if (source == null)
                grid.UsingFooter("_Loader");
            else
                grid.Empty("Нет данных");

            return grid;
        }

        public static void AddNumberCollumn<T>(this IGridColumnsOf<T> columns)
        {
            columns.Add()
                .RenderedAs((model, row) => row + 1 +
                    (columns.Grid.Pager == null ? 0
                    : (columns.Grid.Pager.CurrentPage - 1) * columns.Grid.Pager.RowsPerPage))
                .Titled("#")
                .Css("text-center")
                .Width(40);
        }

        public static void AddSettingButtoms<T>(this IGridColumnsOf<T> columns, IUrlHelper Url,
            string editAction = "Edit", string deleteAction = "Delete")
            where T : BaseTableModel
        {
            columns.AddEditButtom(Url.Action(editAction)!);
            columns.AddRemoveButtom(Url.Action(deleteAction)!);
        }

        public static IGridColumn<T, string> AddEditButtom<T>(this IGridColumnsOf<T> columns, string url) where T : BaseTableModel
        {
            return columns.Add(m =>
                $"<button onclick=\"showEditModal('{url}/{m.Id}')\"" +
                $"class='btn btn-primary-outline fa-shake-hover btn-edit'>" +
                    $"<i class=\"fa-solid fa-pen\"></i>" +
                $"</button>")
            .Encoded(false)
            .Css("col-btn-full")
            .Width(40);
        }

        public static IGridColumn<T, string> AddRemoveButtom<T>(this IGridColumnsOf<T> columns, string url) where T : BaseTableModel
        {
            return columns.Add(m =>
                !m.CanDelete ? string.Empty :
                $"<button " +
                $"data-bs-toggle='modal' data-bs-target='#modal-remove'" +
                $"data-bs-url=\"{url}/{m.Id}\" " +
                $"class='btn btn-primary-outline fa-shake-hover btn-remove'>" +
                    $"<i class=\"fa-regular fa-trash-can\"></i>" +
                $"</button>")
            .Encoded(false)
            .Css("col-btn-full")
            .Width(40);
        }
    }
}
