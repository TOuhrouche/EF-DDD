using System.Collections.Generic;

namespace EF_DDD.Partnership
{
    public class Partner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private readonly List<PartnerEmployee> _employees = new List<PartnerEmployee>();
        public virtual IReadOnlyList<PartnerEmployee> Employees => _employees;
        protected Partner(){}
    }
}
