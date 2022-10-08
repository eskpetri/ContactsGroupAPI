using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace contactgroupAPIefMySQL.Models
{
    public partial class Contact
    {
        public Contact()
        {
            Groupcontacts = new HashSet<Groupcontact>();
        }

        public int Idcontacts { get; set; }
        public string Username { get; set; } = null!;
        public string? Password { get; set; }
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public sbyte? Isadmin { get; set; }

        [JsonIgnore]        //Leave ICollection out of query 
        public virtual ICollection<Groupcontact> Groupcontacts { get; set; }
    }
}
