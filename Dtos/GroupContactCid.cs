using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace contactgroupAPIefMySQL.Models
{
    public class GroupContactCid
    {
        public GroupContactCid()    //This object populated data from cgroup joined with Groupcontact
        {
        }
        public GroupContactCid(Cgroup Cgo, int member, int isadmin)    //This object populated data from cgroup joined with Groupcontact
        {
            this.Idgroups=Cgo.Idgroups;this.Groupname=Cgo.Groupname; this.Description=Cgo.Description;this.ismember=member;this.isadmin=isadmin;
        }
        public int Idgroups { get; set; } = 0;
        public string Groupname { get; set; } = null!;
        public string? Description { get; set; } = string.Empty;

        public int ismember  { get; set; } = 0;  //Shows is contact is member of a group 0 = not 1=yes
        public int isadmin  { get; set; } = 0;  //Shows is contact is admin of that group 0 = not 1=yes

    }
}
