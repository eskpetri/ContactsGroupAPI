using System;
using System.Collections.Generic;

namespace contactgroupAPIefMySQL.Models
{
    public partial class Groupcontact
    {
        public int Idgroupcontacts { get; set; }
        public int? Idcontacts { get; set; }
        public int? Idgroups { get; set; }
        public sbyte? Isadmin { get; set; }

        public virtual Contact? IdcontactsNavigation { get; set; }
        public virtual Cgroup? IdgroupsNavigation { get; set; }
    }
}
