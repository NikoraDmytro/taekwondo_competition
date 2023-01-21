using DALAbstractions;

namespace DAL;

public class UnitOfWork: IUnitOfWork
{
    private ClubRepository? _clubRepository;
    private readonly string _connectionString;
    public UnitOfWork(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IClubRepository ClubRepository => 
        _clubRepository ??= new ClubRepository(_connectionString);
}