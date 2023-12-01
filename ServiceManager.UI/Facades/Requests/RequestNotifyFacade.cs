using ServiceManager.Core;
using ServiceManager.Core.Repositories;
using ServiceManager.UI.Models.Base;
using ServiceManager.UI.Models.Requests;

namespace ServiceManager.UI.Facades.Requests
{
    public class RequestNotifyFacade
    {
        private readonly ServicesContext _ctx;
        private readonly IdentityRepository _identityRepository;
        public RequestNotifyFacade(ServicesContext ctx, IdentityRepository identityRepository)
        {
            _ctx = ctx;
            _identityRepository = identityRepository;
        }

        public int GetCountUnreadByUser()
        {
            var userId = _identityRepository.GetUserId();

            return _ctx.RequestNotify.Count(x => x.UserId == userId && !x.IsRead);
        }

        public void Read(Guid notifyId)
        {
            var entity = _ctx.RequestNotify.First(x => x.Id == notifyId);
            entity.IsRead = true;
            _ctx.SaveChanges();
        }

        public IQueryable<NotifyModel> GetAllByUser()
        {
            var userId = _identityRepository.GetUserId();
            return _ctx.RequestNotify
                .Where(x => x.UserId == userId)
                .Where(x => !x.IsRead)
                .OrderByDescending(x => x.RequestHistory.Date)
                .Select(x => new NotifyModel
                {
                    Id = x.Id,
                    Text = x.Text,
                    IsRead = x.IsRead,
                    Date = x.RequestHistory.Date,
                    Action = x.RequestHistory.Action,
                    RequestText = x.RequestHistory.Request.Text,
                    Executer = x.RequestHistory.User == null ? null :
                        new NamedModel
                        {
                            Id = x.RequestHistory.User.Id,
                            Name = x.RequestHistory.User.ShortName,
                        },
                    Request =
                        new NamedModel
                        {
                            Id = x.RequestHistory.Request.Id,
                            Name = x.RequestHistory.Request.Number.ToString(),
                        }
                });
        }
    }
}
