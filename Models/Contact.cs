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

        public int Idcontacts { get; set; } = 0;
        public string Username { get; set; } = null!;
        public string? Password { get; set; } = string.Empty;
        public string? Nickname { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public sbyte? Isadmin { get; set; } = 0;

        [JsonIgnore]        //Leave ICollection out of query 
        public virtual ICollection<Groupcontact> Groupcontacts { get; set; }
    }
}
