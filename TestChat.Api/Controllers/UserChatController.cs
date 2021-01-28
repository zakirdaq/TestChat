using System;
using System.Threading.Tasks;
using AutoMapper;
using TestChat.Api.SignalR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TestChat.Core.Models.EntityModels;
using TestChat.Core.Services;
using TestChat.Core.Models.DomainModels;
using TestChat.Api.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using TestChat.Core.Models.ViewModels;

namespace TestChat.Api.Controllers
{
    [Route("api/UserChat")]
    [ApiController]
    public class UserChatsController : ControllerBase
    {
        #region Declaration & Construction
        private readonly IUserChatService iUserChatService;
        private readonly IUserService iUserService;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IHubContext<ChatHub> _hubContext;

        public UserChatsController(IUserChatService iUserChatService, IUserService iUserService, IMapper mapper, IWebHostEnvironment hostingEnvironment,
            IHubContext<ChatHub> hubContext)
        {
            this.iUserChatService = iUserChatService;
            this.iUserService = iUserService;
            this._mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _hubContext = hubContext;
        }
        #endregion Declaration & Construction


        #region create
        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> create([FromBody] ChatModel mappingResource)
        {
            ReturnResult returnResult = new ReturnResult();

            if (mappingResource == null)
            {
                returnResult.code = 1;
                returnResult.message = "no data posted";
                return Ok(returnResult);
            }

            if (string.IsNullOrEmpty(mappingResource.Message))
            {
                returnResult.code = 1;
                returnResult.message = "message reqired";
                return Ok(returnResult);
            }

            if (mappingResource.RecieverId == Guid.Empty)
            {
                returnResult.code = 1;
                returnResult.message = "receiver reqired";
                return Ok(returnResult);
            }

            try
            {
                Users Sender = await iUserService.GetByEmail(User.Identity.Name);
                Users Reciever = await iUserService.GetById(mappingResource.RecieverId);

                UserChats userChatObj = new UserChats();
                userChatObj.Id = Guid.NewGuid();
                userChatObj.Message = mappingResource.Message;
                userChatObj.SenderId = Sender.Id;
                userChatObj.RecieverId = Reciever.Id;
                userChatObj.ReadStatus = false;
                userChatObj.MessageTime = DateTime.Now;

                UserChats returnObj = await iUserChatService.Create(userChatObj);

                if (returnObj == null)
                {
                    returnResult.code = 2;
                    returnResult.message = "text_chat_err";
                }
                else
                {
                    returnResult.code = 1;
                    returnResult.message = "text_chat_create_success";
                    await _hubContext.Clients.All.SendAsync("ReceiveOne", userChatObj.Id, userChatObj.SenderId, userChatObj.RecieverId, Sender.FirstName + " " + Sender.LastName + " to "+ Reciever.FirstName + " " + Reciever.LastName, returnObj.MessageTime.ToLongTimeString(), mappingResource.Message);
                }

                return Ok(returnResult);
            }
            catch (Exception ex)
            {
                returnResult.code = 0;
                returnResult.data = null;
                returnResult.message = ex.Message;
                return Ok(returnResult);
            }
        }
        #endregion

        #region remove
        [HttpDelete("Remove/{id}")]
        [Authorize]
        public async Task<IActionResult> remove(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var obj = await iUserChatService.GetById(id);
            if (obj == null)
                return NotFound();

            await iUserChatService.Remove(obj);

            return Ok();
        }
        #endregion remove

        #region GetByUserId
        [HttpGet("GetByUserId/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var uc = (await iUserChatService.GetByUserId(userId));
            var ucv = AutoMapperConfiguration.Mapper.Map<IEnumerable<UserChats>, IEnumerable<UserChatViewModel>>(uc);

            return Ok(ucv);
        }
        #endregion
    }
}