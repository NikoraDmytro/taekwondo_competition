namespace DALAbstractions;

public interface IUnitOfWork
{
    IClubRepository ClubRepository { get; }
}