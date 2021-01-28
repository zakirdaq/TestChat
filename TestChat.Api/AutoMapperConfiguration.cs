using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestChat.Core.Models.EntityModels;
using TestChat.Core.Models.ViewModels;

namespace TestChat.Api
{
    public class AutoMapperConfiguration
    {
        public static void Initialize()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {

                #region TestChat
                #region Users => UserViewModel
                cfg.CreateMap<Users, UserViewModel>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                                                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                                                                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                                                                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                                                                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
                #endregion Users => UserViewModel

                #region UserChats => UserChatViewModel
                cfg.CreateMap<UserChats, UserChatViewModel>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                                                .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.Sender.FirstName + " " + src.Sender.LastName + " to " + src.Reciever.FirstName + " " + src.Reciever.LastName))
                                                                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId))
                                                                .ForMember(dest => dest.RecieverId, opt => opt.MapFrom(src => src.RecieverId))
                                                                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
                                                                .ForMember(dest => dest.MessageTime, opt => opt.MapFrom(src => src.MessageTime.ToLongTimeString()));
                #endregion UserChats => UserChatViewModel
                #endregion TestChat

            });
            Mapper = MapperConfiguration.CreateMapper();
        }

        public static IMapper Mapper { get; private set; }

        public static MapperConfiguration MapperConfiguration { get; private set; }
    }
}
 
 