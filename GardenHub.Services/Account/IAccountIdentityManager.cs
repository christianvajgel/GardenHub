using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GardenHub.Services.Account
{
    public interface IAccountIdentityManager
    {
        Task<SignInResult> Login(string userName, string password);
        Task Logout();
    }
}
