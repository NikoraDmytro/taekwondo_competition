using AutoMapper;
using Core.DataTransferObjects.Competitor;
using Core.Entities;

namespace Api.Profiles;

public class CompetitorProfile: Profile
{
    public CompetitorProfile()
    {
        CreateMap<CreateCompetitorsDto, Competitor>();
    }
}