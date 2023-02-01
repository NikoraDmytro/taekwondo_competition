using System.Data;
using AutoMapper;
using BLLAbstractions;
using Core.DataTransferObjects;
using Core.DataTransferObjects.Club;
using Core.Entities;
using Core.RequestFeatures;
using DALAbstractions;

namespace BLL;

public class ClubsService : BaseService, IClubsService
{

    public ClubsService(IMapper mapper, IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork) { }

    public async Task<IEnumerable<Club>> GetClubs(ClubParameters clubParameters)
    {
        var clubs = await UnitOfWork.ClubRepository.GetClubs(clubParameters);

        return clubs;
    }

    public async Task<Club> GetSingleClub(int clubId)
    {
        var club = await UnitOfWork.ClubRepository.GetById(clubId);

        if (club == null)
        {
            throw new KeyNotFoundException($"Клуб з ID = {clubId} не знайдено!");
        }

        return club;
    }

    public async Task<Club> CreateClub(CreateClubDto createClubDto)
    {
        string clubName = createClubDto.ClubName!;
        var duplicate = await UnitOfWork
            .ClubRepository
            .GetClubByName(clubName);

        if (duplicate != null)
        {
            throw new DuplicateNameException(
                $"Клуб з назвою {clubName} вже існує!"
            );
        }

        var newClub = Mapper.Map<Club>(createClubDto);

        newClub.ClubId = (int)(UInt64)await UnitOfWork
            .ClubRepository
            .Add(newClub);

        return newClub;
    }

    public async Task DeleteClub(int clubId)
    {
        await GetSingleClub(clubId);

        await UnitOfWork
            .ClubRepository
            .Delete(clubId);
    }

    public async Task<Club> UpdateClub(int clubId, CreateClubDto updateClubDto)
    {
        await GetSingleClub(clubId);

        string clubName = updateClubDto.ClubName!;
        var duplicate = await UnitOfWork
            .ClubRepository
            .GetClubByName(clubName);

        if (duplicate != null && duplicate.ClubId != clubId)
        {
            throw new DuplicateNameException(
                $"Клуб з назвою {clubName} вже існує!"
            );
        }

        var updatedClub = Mapper.Map<Club>(updateClubDto);

        await UnitOfWork.ClubRepository.Update(clubId, updatedClub);
        updatedClub.ClubId = clubId;

        return updatedClub;
    }
}