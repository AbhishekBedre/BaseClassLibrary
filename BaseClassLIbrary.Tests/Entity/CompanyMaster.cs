using System.Collections;
using System.Linq.Expressions;

namespace BaseClassLIbrary.Tests.Entity
{
    public class CompanyMaster
    {
        public Int64 Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? GSTNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedUserId { get; set; }

    }
}
