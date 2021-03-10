namespace TestWebService.DA.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Threading.Tasks;
    using TestWebService.DA.Models;

    public class UserRepository : IUserRepository
    {
        private readonly Func<MainContext> dbContextFactory;
        public UserRepository(Func<MainContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using (var context = this.dbContextFactory())
            {
                var retVal = await context.Set<User>().AsNoTracking().ToListAsync();
                return retVal;
            }
        }

        public async Task<User> GetById(long id)
        {
            using (var context = this.dbContextFactory())
            {
                var retVal = await context.Set<User>().FindAsync(id);
                return retVal;
            }
        }

        public async Task Save(User user)
        {
            using (var context = this.dbContextFactory())
            {
                if (user.Id != default)
                {
                    context.Entry(user).State = EntityState.Modified;
                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var exist = await context.Set<User>().AnyAsync(x => x.Id == user.Id);
                        if (!exist)
                        {
                            throw new NotFoundException("User", user.Id, ex);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    context.Set<User>().Add(user);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteById(long id)
        {
            using (var context = this.dbContextFactory())
            {
                User user = await context.Set<User>().FindAsync(id);
                if (user == null)
                {
                    return;
                }

                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }
    }
}