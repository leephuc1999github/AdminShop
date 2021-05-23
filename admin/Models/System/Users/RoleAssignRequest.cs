using System;
using System.Collections.Generic;
using System.Text;
using view_model.Common;

namespace view_model.System.Users
{
    public class RoleAssignRequest
    {
        public Guid Id { get; set; }
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}
