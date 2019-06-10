using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.EntityFrameworkCore;

namespace APIDemo.Models
{
    public class MemberContext : DbContext
    {
        public MemberContext(DbContextOptions<MemberContext> options)
            : base(options)
        {
        }
        public DbSet<Member> Members { get; set; }
    }
}