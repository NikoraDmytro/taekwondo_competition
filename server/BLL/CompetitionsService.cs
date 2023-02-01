using AutoMapper;
using BLLAbstractions;
using Core.DataTransferObjects.Competition;
using Core.DataTransferObjects.Sportsman;
using Core.Entities;
using Core.RequestFeatures;
using DALAbstractions;

namespace BLL;

public class CompetitionsService: BaseService, ICompetitionsService
{
    public CompetitionsService(IMapper mapper, IUnitOfWork unitOfWork) 
        : base(mapper, unitOfWork) { }
    
    public async Task<IEnumerable<Competition>> GetCompetitions(CompetitionParameters competitionParameters)
    {
        var competitions = await UnitOfWork
            .CompetitionRepository
            .GetCompetitions(competitionParameters);

        return competitions;
    }

    public async Task<Competition> GetSingleCompetition(int competitionId)
    {
        var competition = await UnitOfWork
            .CompetitionRepository
            .GetById(competitionId);

        if (competition == null)
        {
            throw new KeyNotFoundException($"Змагання з ID = {competitionId} не знайдено!");
        }
        
        return competition;
    }

    public async Task<Competition> CreateCompetition(CreateCompetitionDto createCompetitionDto)
    {
        var newCompetition = Mapper.Map<Competition>(createCompetitionDto);

        newCompetition.CompetitionId = (int)(UInt64)await UnitOfWork
            .CompetitionRepository
            .Add(newCompetition);

        return newCompetition;
    }

    public async Task DeleteCompetition(int competitionId)
    {
        await GetSingleCompetition(competitionId);
        await UnitOfWork.CompetitionRepository.Delete(competitionId);
    }

    public async Task<Competition> UpdateCompetition(int competitionId, CreateCompetitionDto updateCompetitionDto)
    {
        await GetSingleCompetition(competitionId);

        var updatedCompetition = Mapper.Map<Competition>(updateCompetitionDto);

        await UnitOfWork
            .CompetitionRepository
            .Update(competitionId, updatedCompetition);
        
        updatedCompetition.CompetitionId = competitionId;

        return updatedCompetition;
    }

    public async Task<(IEnumerable<SportsmanDto>, int)> GetAvailableSportsmans(
        int competitionId, 
        SportsmanParameters sportsmanParameters)
    {
        var (sportsmansDto, count) = await UnitOfWork
            .SportsmanRepository
            .GetSportsmansForCompetition(competitionId, sportsmanParameters);

        return (sportsmansDto, GetPageCount(count, sportsmanParameters.PageSize));
    }
}