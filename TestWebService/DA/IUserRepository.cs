namespace TestWebService.DA
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestWebService.DA.Models;

    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();

        Task<User> GetById(long id);

        Task Save(User user);

        Task DeleteById(long id);
    }
}