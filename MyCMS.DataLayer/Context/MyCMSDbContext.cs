using Microsoft.EntityFrameworkCore;
using MyCMS.DomainClasses.Page;
using MyCMS.DomainClasses.PageGroup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace MyCMS.DataLayer.Context
{
    public class MyCMSDbContext:IdentityDbContext
    {
        public MyCMSDbContext(DbContextOptions<MyCMSDbContext> options):base(options)
        {

        }

        public DbSet<Page> Pages { get; set; }
        public DbSet<PageGroup> PageGroups { get; set; }
    }
}
