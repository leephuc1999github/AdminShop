using Microsoft.AspNetCore.Identity;
using System;

namespace library.Models.ESHOP
{
    public class AppRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
