using System;
using System.Collections.Generic;
using System.Text;

namespace EF_DDD
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ManagerId { get; set; }
        public virtual Person Manager { get; set; }
    }
}
