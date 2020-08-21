using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyWebApp.Models;
using StoreOfBuild.Domain.Account;

namespace StoreOfBuild.Data.Identity
{
    public class Authentication : IAuthentication
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        public Authentication(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            var result = await this.signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
            var teste = result;
            return result.Succeeded;
        }
    }
}