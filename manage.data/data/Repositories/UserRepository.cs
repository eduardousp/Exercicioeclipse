using manage.core.entities;
using manage.core.interfaces;
using manage.infra.context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.infra.data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;  

        
        public UserRepository(Context context)
        {
            _context = context;
        }

        
        public async Task<User> GetByUsernameAsync(string username)
        {
        
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == username);
        }

        
        public async Task AddAsync(User user)
        {
        
            await _context.Users.AddAsync(user);

        
            await _context.SaveChangesAsync();
        }

        
        public async Task<bool> UserExistsAsync(string username)
        {
        
            return await _context.Users.AnyAsync(u => u.Email == username);
        }
    }
}
