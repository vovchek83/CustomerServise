using System;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Threading.Tasks;
using CustomersService.DataModel.Models;
using CustomersService.DataModel.Repositories.Interfaces;

namespace CustomersService.DataModel.Repositories
{
    [Export(typeof(IUsersRepository))]
    public class UsersRepository : IUsersRepository
    {

        public async Task<User> GetUserAsync(string name)
        {
            using (var context = new CustomersServiceDbContext())
            {
                return await context.Users.FirstOrDefaultAsync(x => x.Name == name);
            }
        }

    }
}