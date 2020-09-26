using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GardenHub.Services.Account
{
    public interface IAccountService
    {

        // ***** CRUD *****

        // Create User
        Task Save(GardenHub.Domain.Account.Account account);

        // Read (find) User
        Task<GardenHub.Domain.Account.Account> FindById(Guid id);

        // Update User
        Task Update(Domain.Account.Account account);

        // Delete User
        Task<IdentityResult> Delete(Domain.Account.Account user);


        // ***** LOGIN *****

        // Login User - USERNAME and PASSWORD
        Task<Domain.Account.Account> GetAccountByUserNamePassword(string userName, string password);

        IEnumerable<Domain.Account.Account> GetAll();
    }
}
