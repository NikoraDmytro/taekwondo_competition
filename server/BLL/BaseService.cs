using AutoMapper;
using DALAbstractions;

namespace BLL
{
    public abstract class BaseService
    {
        protected readonly IMapper Mapper;
        protected readonly IUnitOfWork UnitOfWork;
        
        protected BaseService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }

        protected int GetPageCount(int count, int pageSize)
        {
            int pageCount = count / pageSize;

            if (count % pageSize != 0)
            {
                pageCount++;
            }
            
            return pageCount;
        }
    } 
}