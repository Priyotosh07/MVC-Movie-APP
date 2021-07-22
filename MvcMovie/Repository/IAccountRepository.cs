using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using MvcMovie.Models;

namespace MvcMovie.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignupUser userModel);
        Task<SignInResult> PasswordLoginAsync(LoginUser loginUser);
        Task SignOut();
    }
}