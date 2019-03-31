using System.Threading.Tasks;
using SQLWorker.DAL.Repositories.Records;

namespace SQLWorker.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<long> SaveUserAsync(User userData);
    }
}