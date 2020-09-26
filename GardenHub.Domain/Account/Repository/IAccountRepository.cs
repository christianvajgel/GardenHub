using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GardenHub.Domain.Account.Repository
{
    public interface IAccountRepository
    {

        // ***** CRUD *****

        // Create User
        Task<IdentityResult> CreateAsync(Domain.Account.Account user, CancellationToken cancellationToken);

        // Read (find) User
        Task<Account> FindByIdAsync(Guid userId, CancellationToken cancellationToken);

        // Update User
        Task<IdentityResult> UpdateAccountAsync(Domain.Account.Account account, CancellationToken cancellationToken);

        // Delete User
        Task<IdentityResult> DeleteAsync(Domain.Account.Account user, CancellationToken cancellationToken);


        // ***** LOGIN *****

        // Login User - EMAIL and PASSWORD
        Task<Account> GetAccountByEmailPassword(string email, string password);

        // Login User - USERNAME and PASSWORD
        Task<Domain.Account.Account> GetAccountByUserNamePassword(string userName, string password);

        IEnumerable<Domain.Account.Account> GetAll();
    }
}
