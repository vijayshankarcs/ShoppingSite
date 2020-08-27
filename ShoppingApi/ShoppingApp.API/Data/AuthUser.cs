using Microsoft.EntityFrameworkCore;
using ShoppingApp.API.Models;
using System;
using ShoppingApp.API.Data;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

namespace ShoppingApp.API.Data
{
    public class AuthUser : IAuthUser
    {
        private readonly DataContext _context;

        public AuthUser(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string UserName, string Password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == UserName);
            if (user == null)
                return null;
            if (!VerifyPasswordHash(Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;

        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmc = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

                var computedHash = hmc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }

            }
            return true;
        }

        public async Task<User> Resgister(User user, string Password)
        {
            byte[] passwordHash, PasswordSalt;
            CreatePasswordHash(Password, out passwordHash, out PasswordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = PasswordSalt;
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmc = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmc.Key;
                passwordHash = hmc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        public async Task<bool> UserExists(string UserName)
        {
            if (await _context.Users.AnyAsync(x => x.UserName == UserName))
                return true;

            return false;
        }
    }
}
