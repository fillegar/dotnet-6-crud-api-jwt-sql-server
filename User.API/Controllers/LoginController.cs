using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using User.API.CustomExceptions;
using User.API.Handler;
using User.Core;
using User.Core.Model;
using User.Service;

namespace User.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserInfoService userInfoService;
        private readonly TokenUtilsHandler tokenHandler;
        public LoginController(UserInfoService userInfoService, TokenUtilsHandler tokenHandler)
        {
            this.userInfoService = userInfoService;
            this.tokenHandler = tokenHandler;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<Token> Login([FromForm] UserLogin userLogin)
        {
            UserInfo user =  await userInfoService.getUserInfoByEmail(userLogin.Email);
            if (user != null)
            {
                var token = tokenHandler.createAccessToken(user);

                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);

                await userInfoService.updateUserInfo(user);

                return token;
            }
            throw new UserInfoNotFoundException("User not found with email: " + userLogin.Email);
        }

        
    }
}
