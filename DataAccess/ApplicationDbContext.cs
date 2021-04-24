using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;
using static Assignment4.Models.EF_Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
//using System.Data.Entity;

namespace Assignment4.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Rootobject> Rootobjects { get; set; }
        public DbSet<Pagination> Paginations { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<ORI> Agencies { get; set; }
        //public DbSet<SignUp> SignUp { get; set; }

        //public DbSet<Results> Results { get; set; }

    }
}