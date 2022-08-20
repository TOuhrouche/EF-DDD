using System;
using System.Collections.Generic;
using System.Text;

namespace EF_DDD
{
    public class Contractor : Person
    {
        public int ContractorId { get; set; }
        public virtual ContractingCompany Company { get; set; }
    }
}
