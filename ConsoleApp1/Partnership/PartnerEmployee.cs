using System.Collections.Generic;
using System.Linq;

namespace EF_DDD.Partnership
{
    public class PartnerEmployee
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public virtual PartnerEmployee Manager { get; set; }
        public virtual Partner Partner { get; set; }
        private List<PartnerEmployee> _reports = new List<PartnerEmployee>();
        public virtual IReadOnlyList<PartnerEmployee> Reports => _reports.ToList();
    }
}
