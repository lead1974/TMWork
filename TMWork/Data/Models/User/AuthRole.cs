using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMWork.Data.Models.User
{
    public class AuthRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
