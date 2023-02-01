using System.Data;
using AutoMapper;
using BLLAbstractions;
using Core.DataTransferObjects.Competitor;
using Core.Entities;
using Core.Exceptions;
using Core.RequestFeatures;
using DALAbstractions;

namespace BLL;

public class CompetitorsService: BaseService, ICompetitorsService
{
    public CompetitorsService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }
    
    public async Task<(IEnumerable<CompetitorDto>, int)> GetCompetitors(
        int competitionId,
        CompetitorParameters competitorParameters)
    {
        var (competitorsDto, total) = await UnitOfWork
            .CompetitorRepository
            .GetCompetitors(competitionId, competitorParameters);
        
        return (competitorsDto, GetPageCount(total, competitorParameters.PageSize));
    }
    
    public async Task<CompetitorDto> GetSingleCompetitor(int applicationNum)
    {
        var competitorDto = await UnitOfWork
            .CompetitorRepository
            .GetCompetitor(applicationNum);

        if (competitorDto == null)
        {
            throw new KeyNotFoundException(
                $"Заявку з номером {applicationNum} не знайдено!");
        }
        
        return competitorDto;
    }

    public async Task CreateCompetitor(CreateCompetitorsDto createCompetitorDto)
    {
        var membershipCardNums =  createCompetitorDto.MembershipCardNums.ToArray();

        if (membershipCardNums.Length == 0)
        {
            throw new AppException("Не вказано жодного номеру членського квитка!");
        }

        int competitionId = (int) createCompetitorDto.CompetitionId!;
        
        var competition = await UnitOfWork
             .CompetitionRepository
             .GetById(competitionId);
        
         if (competition == null)
         { 
             throw new KeyNotFoundException($"Змагання з ідентифікатором {competitionId} не знайдено!");
         }
        
         var sportsmans = await UnitOfWork
             .SportsmanRepository
             .GetByIds(membershipCardNums.Cast<object>().ToArray());
         var sportsmansArr = sportsmans.ToArray();
         var existingCardNums = sportsmansArr.Select(s => s.MembershipCardNum).ToArray();

         var notFound = membershipCardNums
             .Where(mcn => !existingCardNums.Contains(mcn))
             .ToArray();

         if (notFound.Length != 0)
         {
             throw new KeyNotFoundException(
                 $"Спортсменів з номером членських квитків {String.Join(", ", notFound)} не знайдено!");
         }
         
         var duplicate = await UnitOfWork
             .CompetitorRepository
             .GetByMembershipCardNums(competitionId, membershipCardNums);
         var duplicateArr = duplicate.ToArray();
         
         if (duplicateArr.Length != 0)
        {
            throw new DuplicateNameException(
                $"Спортсмен з номером членського квитка {String.Join(", ", duplicateArr.Select(s => s.MembershipCardNum))} вже зареєстрований на це змагання!"
            );
        }

        var newCompetitors = new List<Competitor>();
         
        foreach (var membershipCardNum in createCompetitorDto.MembershipCardNums)
        {
            newCompetitors.Add( new Competitor()
            {
                MembershipCardNum = membershipCardNum,
                CompetitionId = competitionId,
                Belt = sportsmansArr.First(s => s.MembershipCardNum == membershipCardNum).Belt
            });
        }
        
        await UnitOfWork
            .CompetitorRepository
            .CreateCompetitors(newCompetitors.ToArray());
    }


    public async Task DeleteCompetitor(int applicationNum)
    {
        await GetSingleCompetitor(applicationNum);
        await UnitOfWork.CompetitorRepository.Delete(applicationNum);
    }
}