using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestChat.Core.Models.EntityModels;
using TestChat.Core.Models.DomainModels;
using TestChat.Api.Model;
using TestChat.Core.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
using TestChat.Api.Validations;
using TestChat.Core.Models.ViewModels;

namespace TestChat.Api.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserChatService _UserChatService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IUserChatService UserChatService, IMapper mapper)
        {
            this._userService = userService;
            this._UserChatService = UserChatService;
            this._mapper = mapper;
        }

        #region SignUp
        [HttpPost("Signup")]
        public async Task<IActionResult> SignUp(SignUpModel mappingResource)
        {
            ReturnResult returnResult = new ReturnResult();

            try
            {
                var validator = new SignUpModelValidation();
                var validatorResult = await validator.ValidateAsync(mappingResource);

                if (!validatorResult.IsValid)
                {
                    returnResult.code = 0;
                    returnResult.message = validatorResult.Errors.ToString();
                    return Ok(returnResult);
                }

                Users userObj = await _userService.GetByEmail(mappingResource.Email);
                if (userObj != null)
                {
                    returnResult.code = 0;
                    returnResult.message = "Sorry! " + mappingResource.Email + " is already registered";
                    return Ok(returnResult);
                }

                userObj = new Users();
                userObj.Id = Guid.NewGuid();
                userObj.FirstName = mappingResource.FirstName;
                userObj.LastName = mappingResource.LastName;
                userObj.Email = mappingResource.Email;
                userObj.Status = true;

                Users returnObj = await _userService.Create(userObj);

                if (returnObj == null)
                {
                    returnResult.code = 2;
                    returnResult.message = "text_user_err";
                }
                else
                {
                    returnResult.code = 1;
                    returnResult.message = "text_user_signup_success";
                }
            }
            catch (Exception ex)
            {
                returnResult.code = 0;
                returnResult.message = ex.Message;
            }

            return Ok(returnResult);
        }
        #endregion SignUp

        #region logout
        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> logout()
        {
            ReturnResult returnResult = new ReturnResult();

            Users users = await _userService.GetByEmail(User.Identity.Name);

            if (users == null)
            {
                return Unauthorized();
            }

            returnResult.data = null;
            returnResult.code = 1;
            returnResult.message = "user_logout";

            return Ok(returnResult);
        }
        #endregion logout        


        #region GetAll
        [HttpGet("GetAll")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAll()
        {
            var objList = (await _userService.GetAll()).Where(x=>x.Email != User.Identity.Name);
            var objListMap = AutoMapperConfiguration.Mapper.Map<IEnumerable<Users>, IEnumerable<UserViewModel>>(objList);
            return Ok(objListMap);
        }
        #endregion GetAll

        #region GetById
        [HttpGet("GetById/{id}")]
        [Authorize]
        public async Task<ActionResult<UserViewModel>> GetById(Guid id)
        {
            var obj = await _userService.GetById(id);
            var objMapper = AutoMapperConfiguration.Mapper.Map<Users, UserViewModel>(obj);
            return Ok(objMapper);
        }
        #endregion GetById
    }
}
 
