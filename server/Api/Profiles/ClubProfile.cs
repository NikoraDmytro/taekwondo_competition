using AutoMapper;
using Core.DataTransferObjects;
using Core.DataTransferObjects.Club;
using Core.Entities;

namespace Api.Profiles;

public class ClubProfile: Profile
{
    public ClubProfile()
    {
        CreateMap<CreateClubDto, Club>();
    }
}