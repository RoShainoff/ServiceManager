using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Core;
using ServiceManager.Core.Entities.Requests;
using ServiceManager.Core.Enums;
using ServiceManager.UI.Facades.Base;
using ServiceManager.UI.Models.Requests;
using ServiceManager.UI.Models.Requests.Report;
using System.Security.Claims;

namespace ServiceManager.UI.Facades.Requests
{
    public class RequestFacade : BaseFacadeAsync<
        RequestModel,
        RequestModel,
        RequestTableModel,
        Request>
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public RequestFacade(ServicesContext ctx,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(ctx, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public override async Task<RequestModel> GetById(Guid id)
        {
            return await _ctx.Request
                .Select(x => new RequestModel
                {
                    Id = x.Id,
                    Number = x.Number,
                    Text = x.Text,
                    Priority = x.Priority,
                    ClientId = x.ClientId,
                    ExecutorId = x.ExecutorId,
                    ServiceTypeId = x.ServiceTypeId,
                    ServiceId = x.ServiceId,

                    Status = x.Histories
                        .Where(x => x.Action != RequestAction.Edit)
                        .OrderBy(x => x.Date)
                        .Last().Action,

                    Materials = x.RequestMaterials.Select(m => 
                        new MaterialCountModel
                        {
                            Name = m.Material.Name,
                            Count = m.Count
                        }).ToList()
                })
                .FirstAsync(x => x.Id == id);
        }


        public override IQueryable<RequestTableModel> Table()
        {
            return _ctx.Request
                .Select(x => new RequestTableModel
                {
                    Id = x.Id,
                    Number = x.Number,
                    Text = x.Text,

                    Priority = x.Priority,
                    Service = x.Service == null ? null : x.Service.Name,
                    ServiceType = x.ServiceType == null ? null : x.ServiceType.Name,

                    Client = x.Client.FullName,
                    ClientId = x.Client.Id,
                    Executer = x.Executor == null ? null : x.Executor.FullName,
                    ExecuterId = x.Executor == null ? null : x.Executor.Id,

                    Status = x.Histories
                        .Where(x => x.Action != RequestAction.Edit)
                        .OrderBy(x => x.Date)
                        .Last().Action,

                    CreateDate = x.Histories
                        .Where(x => x.Action != RequestAction.Edit)
                        .OrderBy(x => x.Date)
                        .First().Date,

                    LastEditDate = x.Histories
                        .OrderBy(x => x.Date)
                        .Last().Date,
                });
        }

        public async Task<Guid> Save(RequestModel model, bool notifyAfterSave)
        {
            var oldEntity = await _ctx.Request.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);
            if (oldEntity == null)
                model.Number = await _ctx.Request.CountAsync() + 1;

            var requestId = await base.Save(model);

            if (notifyAfterSave)
                await NotifyAfterSave(oldEntity, model);

            return requestId;
        }

        public async override Task<Guid> Save(RequestModel model)
        {
            return await Save(model, notifyAfterSave: true);
        }

        private async Task NotifyAfterSave(Request? oldEntity, RequestModel model)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            Guid historyId;
            if (oldEntity == null)
            {
                historyId = await Execute(new RequestExecuteModel
                {
                    RequestId = model.Id,
                    UserId = userId,
                    Action = RequestAction.Create
                });

                if (model.ExecutorId.HasValue && model.ExecutorId != userId)
                    await Notify(historyId, model.ExecutorId.Value, "Вам назначили заявку");
            }
            else
            {
                historyId = await Execute(new RequestExecuteModel
                {
                    RequestId = model.Id,
                    UserId = userId,
                    Action = RequestAction.Edit
                });

                if (model.ExecutorId != oldEntity.ExecutorId)
                {
                    if (model.ExecutorId.HasValue && model.ExecutorId != userId)
                        await Notify(historyId, model.ExecutorId.Value, "Вам назначили заявку");
                    if (oldEntity.ExecutorId.HasValue && oldEntity.ExecutorId != userId)
                        await Notify(historyId, oldEntity.ExecutorId.Value, "Вас открепили от заявки");
                }

                if (model.ClientId != userId)
                    await Notify(historyId, model.ClientId, "Ваша заявка была измененна");
            }
        }

        public async Task AddMaterials(Guid requestId, List<RequestMaterialModel> materials)
        {
            var entities = materials.Select(x => new RequestMaterial
            {
                RequestId = requestId,
                MaterialId = x.MaterialId,
                Count = x.Count,
            });
            await _ctx.AddRangeAsync(entities);
            await _ctx.SaveChangesAsync();
        }

        public async Task<Guid> Execute(RequestExecuteModel model)
        {
            var entity = _mapper.Map<RequestHistory>(model);
            _ctx.Add(entity);
            await _ctx.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<Guid> Notify(Guid historyId, Guid userId, string text = "")
        {
            var entity = new RequestNotify
            {
                RequestHistoryId = historyId,
                UserId = userId,
                Text = text,
            };
            _ctx.Add(entity);
            await _ctx.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<RequestAction[]> GetNextTransactions(Guid requestId)
        {
            var status = await _ctx.RequestHistory
                .Where(x => x.RequestId == requestId)
                .Where(x => x.Action != RequestAction.Edit)
                .OrderBy(x => x.Date)
                .Select(x => x.Action)
                .LastAsync();

            return status switch
            {
                RequestAction.Create => new RequestAction[]
                {
                    RequestAction.Accept,
                    RequestAction.Close,
                },

                RequestAction.Accept or
                RequestAction.Expired => new RequestAction[]
                {
                    RequestAction.GoodComplete,
                    RequestAction.BadComplete,
                    RequestAction.Close,
                },

                _ => new RequestAction[] { },
            };
        }

        public IEnumerable<ReportAgentServiceType> GetForReport()
        {
            return _ctx.Users
                .Where(user => user.Client == null)
                .Select(x => new ReportAgentServiceType
                {
                    FullName = x.FullName,

                    ServiceTypes = _ctx.ServiceType.Select(y => new ReportAgentServiceTypeRow
                    {
                        Name = y.Name,

                        CountAll = x.ExecutedRequests
                            .Count(report => report.Histories
                                .All(history => history.Action != RequestAction.Close)),

                        CountBadComplited = x.ExecutedRequests
                            .Count(report => report.Histories
                                .Any(history => history.Action == RequestAction.BadComplete)),

                        CountGoodComplited = x.ExecutedRequests
                            .Count(report => report.Histories
                                .Any(history => history.Action == RequestAction.GoodComplete)),

                    }).Where(x => x.CountAll > 0).ToList()
                }).Where(x => x.ServiceTypes.Count() > 0);
        }
    }
}
