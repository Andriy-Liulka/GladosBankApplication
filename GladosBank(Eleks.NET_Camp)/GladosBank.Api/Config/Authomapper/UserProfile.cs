using AutoMapper;
using GladosBank.Domain;
using GladosBank.Domain.Models_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Config.Authomapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<UserDTO, User>();
        }
    }
}
