using AutoMapper;
using BLLAbstractions;
using Core.DataTransferObjects;
using Core.DataTransferObjects.Sportsman;
using Core.Entities;
using Core.RequestFeatures;
using DALAbstractions;

namespace BLL;

public class SportsmansService: BaseService, ISportsmansService
{
    public SportsmansService(IMapper mapper, IUnitOfWork unitOfWork) 
        : base(mapper, unitOfWork) { }

    public async Task<(IEnumerable<SportsmanDto>, int)> GetSportsmans(SportsmanParameters sportsmanParameters)
    {
        var (sportsmansDto, count) = await UnitOfWork
            .SportsmanRepository
            .GetSportsmans(sportsmanParameters);

        return (sportsmansDto, GetPageCount(count, sportsmanParameters.PageSize));
    }

    public async Task<SportsmanDto> GetSingleSportsman(int membershipCardNum)
    {
        var sportsmanDto = await UnitOfWork
            .SportsmanRepository
            .GetSportsman(membershipCardNum);

        if (sportsmanDto == null)
        {
            throw new KeyNotFoundException($"Спортсмена з номером членського квитка {membershipCardNum} не знайдено!");
        }
        
        return sportsmanDto;
    }

    public async Task<SportsmanDto> CreateSportsman(CreateSportsmanDto createSportsmanDto)
    {
        var newSportsman = Mapper.Map<Sportsman>(createSportsmanDto);
        
        var membershipCardNum = (int)(UInt64) await UnitOfWork
            .SportsmanRepository
            .Add(newSportsman);

        var newSportsmanDto = await GetSingleSportsman(membershipCardNum);
        
        return newSportsmanDto; 
    }

    public async Task DeleteSportsman(int clubId)
    {
        await GetSingleSportsman(clubId);
        await UnitOfWork.SportsmanRepository.Delete(clubId);
    }

    public async Task<SportsmanDto> UpdateSportsman(int membershipCardNum, CreateSportsmanDto updateSportsmanDto)
    {
        await GetSingleSportsman(membershipCardNum);
        
        var updatedSportsman = Mapper.Map<Sportsman>(updateSportsmanDto);

        await UnitOfWork.SportsmanRepository.Update(membershipCardNum, updatedSportsman);

        var updatedSportsmanDto = await GetSingleSportsman(membershipCardNum);
        
        return updatedSportsmanDto;
    }

}