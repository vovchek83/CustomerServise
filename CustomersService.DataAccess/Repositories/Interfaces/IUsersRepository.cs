using System.Threading.Tasks;
using CustomersService.DataModel.Models;

namespace CustomersService.DataModel.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> GetUserAsync(string name);

    }
}