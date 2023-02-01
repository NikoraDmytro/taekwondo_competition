using DALAbstractions;

namespace DAL;

public class UnitOfWork: IUnitOfWork
{
    private ClubRepository? _clubRepository;
    private SportsmanRepository? _sportsmanRepository;
    private CompetitorRepository? _competitorRepository;
    private CompetitionRepository? _competitionRepository;
    
    private readonly string _connectionString;
    
    public UnitOfWork(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IClubRepository ClubRepository => 
        _clubRepository ??= new ClubRepository(_connectionString);
    public ISportsmanRepository SportsmanRepository => 
        _sportsmanRepository ??= new SportsmanRepository(_connectionString);
    public ICompetitorRepository CompetitorRepository => 
        _competitorRepository ??= new CompetitorRepository(_connectionString);
    public ICompetitionRepository CompetitionRepository => 
        _competitionRepository ??= new CompetitionRepository(_connectionString);
}