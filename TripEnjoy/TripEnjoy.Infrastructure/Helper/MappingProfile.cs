using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountDTO>()
            .ForMember(dest => dest.email, otp => otp.MapFrom(src => src.AccountEmail))
            .ReverseMap()
            .ForMember(dest => dest.AccountEmail, opt => opt.MapFrom(src => src.email))
            .ForMember(dest => dest.AccountPassword, opt => opt.MapFrom(src => src.password))
            .ReverseMap()
            .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.AccountPassword));

            // Mapping Booking
            CreateMap<Booking, BookingDTO>().ReverseMap();
            // Mapping Profile
            CreateMap<Account, UserProfile>().ReverseMap();
            // Mapping Room
            CreateMap<Room, RoomDTO>().ReverseMap();
            // Mapping Rate
            CreateMap<Rate, RateDTO>().ReverseMap();
            // Mapping Comment
            CreateMap<Comment, CommentDTO>().ReverseMap();
            // Mapping Commnet Response
            CreateMap<Comment, CommentResponseDTO>().ReverseMap();
            // mapping Transaction
            CreateMap<TransactionHistory, TransactionDTO>().ReverseMap();
            // Mapping Voucher
            CreateMap<Voucher, VoucherDTO>().ReverseMap();
            // Mapping BookingOfPartnerResponse
            CreateMap<Booking, BookingOfPartnerResponse>().ReverseMap();
            // Mapping Room
            CreateMap<Room, RoomPartner>().ReverseMap();
        }
    }
}
