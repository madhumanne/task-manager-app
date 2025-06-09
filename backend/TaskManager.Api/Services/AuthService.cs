using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Api.Models;
using TaskManager.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Api.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> SignupAsync(string name, string email, string phone, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return (false, "Email already exists.");

            var user = new User
            {
                Name = name,
                Email = email,
                Phone = phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool Success, string Message, Guid? SessionId, User User)> LoginAsync(string email, string password, DateTime expiry)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return (false, "Invalid credentials.", null, null);

            var sessionId = Guid.NewGuid();
            var session = new Session
            {
                SessionId = sessionId,
                UserId = user.Id,
                Expiry = expiry,
                IsExpired = false
            };
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return (true, null, sessionId, user);
        }

        public async Task LogoutAsync(Guid sessionId)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.SessionId == sessionId && !s.IsExpired);
            if (session != null)
            {
                session.IsExpired = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
