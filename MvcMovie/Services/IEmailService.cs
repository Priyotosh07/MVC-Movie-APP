using MvcMovie.Models;
using System.Threading.Tasks;

namespace MvcMovie.Services
{
    public interface IEmailService
    {
        Task SendTestemail(UserEailOptions userEailOptions);
    }
}