using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using TestChat.Core.Models.EntityModels;
using TestChat.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestChat.Core.Models.DomainModels;

namespace TestChat.Api.Controllers
{
    [Route("api/Token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        #region Declaration & Construction
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;
        private IHttpContextAccessor _contextAccessor;
        public TokenController(IUserService userService, IMapper mapper, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor)
        {
            this._userService = userService;
            this._mapper = mapper;
            this._hostingEnvironment = hostingEnvironment;
            this._contextAccessor = contextAccessor;
        }
        #endregion Declaration & Construction

        [HttpPost]
        public async Task<IActionResult> Create(SignInModel model)
        {
            Users userObj = await _userService.GetByEmail(model.Email);

            if (userObj != null)
            {
                if(userObj.Status == null || !userObj.Status.Value)
                {
                    return BadRequest(new { message = "User Is Inactive" });
                }

                string token = GenerateToken(model.Email);
                LoginResponseModel user = new LoginResponseModel();
                user.Id = userObj.Id;
                user.token = token;
                user.UserName = userObj.FirstName + " " + userObj.LastName;

                return new ObjectResult(user);
            }

            return BadRequest(new { message = "Email is incorrect" });
        }

        private string GenerateToken(string username)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsATestChatApp")),
                                             SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            option.IsEssential = true;

            Response.Cookies.Append(key, value, option);
        }
    }

    public class LoginResponseModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string token { get; set; }
    }
}