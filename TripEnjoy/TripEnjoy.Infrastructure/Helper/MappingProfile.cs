using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           CreateMap<Account , AccountDTO>().ReverseMap();
        }
    }
}
