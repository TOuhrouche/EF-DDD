using System;
using System.Collections.Generic;
using System.Text;

namespace EF_DDD
{
    public class ContractingCompany
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        private readonly List<Contractor> _contractors = new List<Contractor>();
        public virtual IReadOnlyList<Contractor> Contractors => _contractors;

        protected ContractingCompany()
        {

        }

        public ContractingCompany(string name) : this()
        {
            Name = name;
        }

        public void Add(Contractor contractor)
        {
            _contractors.Add(contractor);
        }
    }
}
