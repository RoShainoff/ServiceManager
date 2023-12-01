using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Core;
using ServiceManager.Core.Entities;
using ServiceManager.Core.Entities.Identity;
using ServiceManager.Core.Enums;
using ServiceManager.Core.Extensions;
using ServiceManager.Core.Models.Identity;
using ServiceManager.Core.Repositories;
using ServiceManager.UI.Facades.Base;
using ServiceManager.UI.Models.Identity;

namespace ServiceManager.UI.Facades.Services
{
    public class ClientFacade : BaseFacadeAsync<
        ClientModel,
        ClientModel,
        ClientTableModel,
        BaseEntity>
    {
        private IdentityRepository _identityRepository;
        private UserManager<User> _userManager;

        public ClientFacade(ServicesContext ctx, IMapper mapper,
            IdentityRepository identityRepository,
            UserManager<User> userManager) 
            : base(ctx, mapper)
        {
            _identityRepository = identityRepository;
            _userManager = userManager;
        }

        public override IQueryable<ClientTableModel> Table()
        {
            return _ctx.Users
                .AsNoTracking()
                .Include(x => x.Client)
                .Include(x => x.ClientRequests)
                    .ThenInclude(x => x.Histories)
                .Where(x => x.Client != null)
                .Select(x => new ClientTableModel
                {
                    Id = x.Id,

                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    RoomName = x.Client!.RoomName,

                    FullName = x.LastName
                        + (string.IsNullOrEmpty(x.FirstName) ? "" : $" {x.FirstName}")
                        + (string.IsNullOrEmpty(x.Patronymic) ? "" : $" {x.Patronymic}"),

                    RequestsAllCount = x.ClientRequests.Count(),

                    RequestsComplitedCount = x.ClientRequests
                        .Where(x => x.Histories.Any(y =>
                            y.Action == RequestAction.GoodComplete ||
                            y.Action == RequestAction.BadComplete)).Count(),

                    RequestsWorkCount = x.ClientRequests
                        .Where(x => x.Histories.Any(y =>
                            y.Action == RequestAction.Accept ||
                            y.Action == RequestAction.Expired)).Count(),
                });
        }

        public override async Task<ClientModel> GetById(Guid id) =>
            await _identityRepository.GetClient(id);

        public override async Task<Guid> Save(ClientModel model)
        {
            if (model.User.Id == default)
                return await Create(model);
            else
                return await Edit(model);
        }

        private async Task<Guid> Create(ClientModel model)
        {
            var result = await _identityRepository.CreateUser(model.User, model.Client);
            result.ExceptionIfFailed();

            model.Client.Id = model.User.Id;
            await _identityRepository.SaveClientData(model.Client);
            return model.User.Id;
        }

        private async Task<Guid> Edit(ClientModel model)
        {
            var result = await _identityRepository.SaveUserData(model.User);
            result.ExceptionIfFailed();

            await _identityRepository.SaveClientData(model.Client);
            return model.User.Id;
        }

        public override async Task Delete(Guid id)
        {
            var entity = _ctx.Users.Find(id);
            if (entity == null)
                throw new InvalidOperationException();
            await _userManager.DeleteAsync(entity);
        }

        public IEnumerable<SelectListItem> Lookup()
        {
            return _ctx.Users
                .Include(x => x.Client)
                .Where(x => x.Client != null)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FullName,
                });
        }
    }
}
