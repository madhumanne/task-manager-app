using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Api.Data;

namespace TaskManager.Api.Middleware
{
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext db)
        {
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && !path.StartsWith("/api/auth/login") && !path.StartsWith("/api/auth/signup"))
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var sessionIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "sessionid");
                    if (sessionIdClaim != null && Guid.TryParse(sessionIdClaim.Value, out var sessionId))
                    {
                        var session = await db.Sessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);
                        if (session == null || session.IsExpired || session.Expiry < DateTime.UtcNow)
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Session expired or invalid.");
                            return;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Session required.");
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Authentication required.");
                    return;
                }
            }
            await _next(context);
        }
    }
}
