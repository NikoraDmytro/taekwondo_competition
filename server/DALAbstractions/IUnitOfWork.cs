namespace DALAbstractions;

public interface IUnitOfWork
{
    IClubRepository ClubRepository { get; }
    ISportsmanRepository SportsmanRepository { get; }
    ICompetitorRepository CompetitorRepository { get; }
    ICompetitionRepository CompetitionRepository { get; }
}