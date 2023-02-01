using AutoMapper;
using Core.DataTransferObjects;
using Core.DataTransferObjects.Sportsman;
using Core.Entities;

namespace Api.Profiles;

public class SportsmanProfile: Profile
{
    public SportsmanProfile()
    {
        CreateMap<CreateSportsmanDto, Sportsman>();
    }
}