using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace contactgroupAPIefMySQL.Models
{
    public partial class Cgroup
    {
        public Cgroup()
        {
            Groupcontacts = new HashSet<Groupcontact>();
        }

        public int Idgroups { get; set; } = 0;
        public string Groupname { get; set; } = null!;
        public string? Description { get; set; } = string.Empty;

        [JsonIgnore]        //Leave ICollection out of query 
        public virtual ICollection<Groupcontact> Groupcontacts { get; set; }
    }
}
