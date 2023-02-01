using AutoMapper;
using Core.DataTransferObjects.Competition;
using Core.Entities;

namespace Api.Profiles;

public class CompetitionProfile: Profile
{
    public CompetitionProfile()
    {
        CreateMap<CreateCompetitionDto, Competition>();
    }
}