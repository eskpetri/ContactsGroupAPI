using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace contactgroupAPIefMySQL.Models
{
    public partial class Groupcontact
    {
        public int Idgroupcontacts { get; set; } = 0;//reduce problems
        public int? Idcontacts { get; set; } = 0;
        public int? Idgroups { get; set; } = 0;
        public sbyte? Isadmin { get; set; } = 0;
        [JsonIgnore]        //Leave ICollection out of query 
        public virtual Contact? IdcontactsNavigation { get; set; }
        [JsonIgnore]        //Leave ICollection out of query 
        public virtual Cgroup? IdgroupsNavigation { get; set; }
    }
}
