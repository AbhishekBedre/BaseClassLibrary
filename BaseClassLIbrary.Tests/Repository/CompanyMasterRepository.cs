using BaseClassLibrary.Interface;
using BaseClassLIbrary.Tests.Entity;
using System.Linq.Expressions;

namespace BaseClassLIbrary.Tests.Repository
{
    public interface ICompanyMasterRepository
    {
        Task<List<CompanyMaster>> GetAllCompanies();
        Task<CompanyMaster> AddCompanyAsync(CompanyMaster companyMaster);
        Task<List<CompanyMaster>> GetQuery(int pageIndex, int pageSize);
    }
    public class CompanyMasterRepository : ICompanyMasterRepository
    {
        private readonly IBaseRepository<CompanyMaster, AppDbContext> _companyMasterRepo;

        public CompanyMasterRepository(IBaseRepository<CompanyMaster, AppDbContext> companyMasterRepo)
        {
            _companyMasterRepo = companyMasterRepo;
        }

        public async Task<CompanyMaster> AddCompanyAsync(CompanyMaster company)
        {
            try
            {
                await _companyMasterRepo.BeginTransactionAsync();

                var result = await _companyMasterRepo.AddAsync(company);

                var result1 = await _companyMasterRepo.AddAsync(new CompanyMaster
                {
                    Name = "Static",
                    Address = "GOTO Hell",
                    CreatedBy = "1",
                    GSTNo = "DSFDSF564",                    
                });

                int m = 25;
                int k = 0;

                int j = m / k;

                await _companyMasterRepo.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _companyMasterRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<CompanyMaster>> GetAllCompanies()
        {
            Expression<Func<CompanyMaster, bool>> predicate = c => c.Id > 4;

            return await _companyMasterRepo.GetAllAsync(predicate);
        }

        public async Task<List<CompanyMaster>> GetQuery(int pageIndex, int pageSize)
        {            
            return await _companyMasterRepo.QueryAsync(
                query => query.Name.Contains("i"),
                orderBy: c => c.Name,
                pageIndex,pageSize);
        }
    }
}
