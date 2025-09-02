using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic; 

namespace ProjectManagerApi.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() { }

        public ApplicationUser(string userName) : base(userName) { }
    }
}