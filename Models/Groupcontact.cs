using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace contactgroupAPIefMySQL.Models
{
    public partial class Groupcontact
    {
        public int Idgroupcontacts { get; set; }
        public int? Idcontacts { get; set; }
        public int? Idgroups { get; set; }
        public sbyte? Isadmin { get; set; }
        [JsonIgnore]        //Leave ICollection out of query 
        public virtual Contact? IdcontactsNavigation { get; set; }
        [JsonIgnore]        //Leave ICollection out of query 
        public virtual Cgroup? IdgroupsNavigation { get; set; }
    }
}
