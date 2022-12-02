using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Sample.IdentityServer.Models
{
    public class AppUser : IdentityUser
    {
        public int Age { get; set; }
    }
}
