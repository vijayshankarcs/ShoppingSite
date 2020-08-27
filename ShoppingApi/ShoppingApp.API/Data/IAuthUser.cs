using ShoppingApp.API.Models;
using System.Threading.Tasks;

namespace ShoppingApp.API.Data
{
    public interface IAuthUser
    {
        Task<User> Resgister(User user, string Password);
        Task<User> Login(string UserName, string Password);
        Task<bool> UserExists(string UserName);
    }
}
