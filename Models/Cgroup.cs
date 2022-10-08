using System;
using System.Collections.Generic;

namespace contactgroupAPIefMySQL.Models
{
    public partial class Cgroup
    {
        public Cgroup()
        {
            Groupcontacts = new HashSet<Groupcontact>();
        }

        public int Idgroups { get; set; }
        public string Groupname { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Groupcontact> Groupcontacts { get; set; }
    }
}
