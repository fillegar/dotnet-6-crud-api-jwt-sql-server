using User.API.Handler;
using User.Service;

namespace User.API.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UserInfoService userService, TokenUtilsHandler tokenUtilsHandler)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var email = tokenUtilsHandler.ValidateToken(token);
            if (!string.IsNullOrEmpty(email))
            {
                context.Items["User"] = userService.getUserInfoByEmail(email);
            }
            await _next(context);
        }
    }
}
