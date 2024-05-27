
using AutoMapper;
using SettlementService.DTOs;
using SettlementService.Entities;

namespace AuctionService.RequestHelpers;

public class MappingProfiles : Profile
{   
    // Automapper logic for conversion of DTO to entity
    public MappingProfiles(){
        CreateMap<CreateBookingDTO, Booking>();
    }
}