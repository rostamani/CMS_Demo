using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyCMS.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCMS.Web.Mappers
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<RegisterViewModel, IdentityUser>().ReverseMap();
        }
    }
}
