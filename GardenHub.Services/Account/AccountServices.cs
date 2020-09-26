using GardenHub.Domain.Account.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GardenHub.Services.Account
{
    public class AccountServices : IAccountService
    {
        private IAccountRepository AccountRepository { get; set; }

        public AccountServices(IAccountRepository accountRepository)
        {
            this.AccountRepository = accountRepository;
        }

        public async Task Save(GardenHub.Domain.Account.Account account)
        {
            await AccountRepository.CreateAsync(account, default);
        }

        public async Task Update(GardenHub.Domain.Account.Account account)
        {
            await AccountRepository.UpdateAccountAsync(account, default);
        }

        public async Task<GardenHub.Domain.Account.Account> FindById(Guid id)
        {
            return await AccountRepository.FindByIdAsync(id, default);
        }

        public async Task<Domain.Account.Account> GetAccountByUserNamePassword(string userName, string password)
        {
            return await AccountRepository.GetAccountByUserNamePassword(userName, password);
        }

        public async Task<IdentityResult> Delete(Domain.Account.Account user)
        {
            return await AccountRepository.DeleteAsync(user, default);
        }

        public IEnumerable<Domain.Account.Account> GetAll() 
        {
            return AccountRepository.GetAll();
        }
    }
}
