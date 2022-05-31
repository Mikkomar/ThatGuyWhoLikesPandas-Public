using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.DAL.Entities
{
    public class AppUserStore : UserStore<User, Role, AppContext, Guid>
    {
        public AppUserStore(DAL.AppContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
