using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.Model
{
    public class DbContext:IdentityDbContext<User,Role,int>
    {

        public DbContext(DbContextOptions<DbContext> options):base(options)
        {

        }
    }
}
