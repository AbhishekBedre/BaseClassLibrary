using BaseClassLibrary.Interface;
using BaseClassLibrary.Repository;
using BaseClassLIbrary.Tests.Entity;
using BaseClassLIbrary.Tests.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BaseClassLIbrary.Tests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ICompanyMasterRepository _companyMasterRepository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            ICompanyMasterRepository companyMasterRepository)
        {
            _logger = logger;
            _companyMasterRepository = companyMasterRepository;
        }

        [HttpGet]
        public async Task<List<CompanyMaster>> Get()
        {
            var result = await _companyMasterRepository.GetAllCompanies();
            return result;
        }

        [HttpGet("GetRow")]
        public async Task<List<CompanyMaster>> GetRow(int pageIndex, int pageSize)
        {
            var result = await _companyMasterRepository.GetQuery(pageIndex, pageSize);
            return result;
        }

        [HttpPost]
        public async Task<CompanyMaster> Post(CompanyMaster companyMaster)
        {
            try
            {                 
                return await _companyMasterRepository.AddCompanyAsync(companyMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}