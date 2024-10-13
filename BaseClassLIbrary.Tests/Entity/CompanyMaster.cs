using BaseClassLibrary.Models;

namespace BaseClassLIbrary.Tests.Entity
{
    public class CompanyMaster : BaseModel
    {
        public Int64 Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? GSTNo { get; set; }        
    }
}
