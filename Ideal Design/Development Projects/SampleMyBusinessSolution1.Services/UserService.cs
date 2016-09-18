using AutoMapper;
using DEAD.DependencyInjection;
using DEAD.DomainPatterns;
using SampleMyBusinessSolution1.Contracts;
using SampleMyBusinessSolution1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMyBusinessSolution1.Services
{
    public class UserService : IUserService,IIoCContexted
    {
        public IIoCContext IoCContext
        {
            get; set;
        }

        public async Task<User> AddUserAsync(User user)
        {
            using (var uow = this.GetIoCManager() .DefualtContainer.Resolve<IUnitOfWork>())
            {
                var repo = uow.GetRepository<User>();
                repo.Add(user);
                await uow.SubmitAsync();
                this.ResolveLogger()?.Debug?.Log("User Added"); 
            }
            return user;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            using (var uow = this.GetIoCManager().DefualtContainer.Resolve<IUnitOfWork>())
            {
                var repou = uow.GetRepository<User>();

                var user = await repou.Entities
                    .Where(x => x.Id == userId)
                    .SingleAsync();
                return user;
            }
        }

        public async Task<User> MoveUserToGroupAsync(int userId, int groupId)
        {
            using (var uow = this.GetIoCManager().DefualtContainer.Resolve<IUnitOfWork>())
            {
                using (var trans = uow.BeginTransaction())
                {
                    var repog = uow.GetRepository<Group>();
                    if (await repog.Entities.AnyAsync(x => x.Id == groupId))
                    {
                        var repou = uow.GetRepository<User>();
                        var user = await repou.Entities
                            .Where(x => x.Id == userId)
                            .SingleAsync();
                        user.GroupId = groupId;

                        await uow.SubmitAsync();
                        return user;
                    }
                    else
                    {
                        throw new Exception("No such group exits");
                    }
                }
            }
        }

        public async Task RemoveUserAsync(User user)
        {
            using (var uow = this.GetIoCManager().DefualtContainer.Resolve<IUnitOfWork>())
            {
                var repo = uow.GetRepository<User>();
                await repo.AttachAndMarkChangesOrAddAsync(user);
                repo.Remove(user);
                await uow.SubmitAsync();
            }

        }

        public async Task<User> UpdateUserAsync(User user)
        {
            using (var uow = this.GetIoCManager().DefualtContainer.Resolve<IUnitOfWork>())
            {
                var repo = uow.GetRepository<User>();
                var target = await repo
                    .Entities.Where(x => x.Id == user.Id)
                    .SingleAsync();
                Mapper.DynamicMap(user, target);
                await uow.SubmitAsync();
                return target;
            }

        }
    }
}
