using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect_WebApp.Models;

namespace Agri_Energy_Connect_WebApp.Data
{
    public class Agri_Energy_Connect_WebAppContext : DbContext
    {
        public Agri_Energy_Connect_WebAppContext (DbContextOptions<Agri_Energy_Connect_WebAppContext> options)
            : base(options)
        {
        }

        public DbSet<Agri_Energy_Connect_WebApp.Models.Employee> Employee { get; set; } = default!;
        public DbSet<Agri_Energy_Connect_WebApp.Models.Farmer> Farmer { get; set; } = default!;
        public DbSet<Agri_Energy_Connect_WebApp.Models.Category> Category { get; set; } = default!;
        public DbSet<Agri_Energy_Connect_WebApp.Models.Product> Product { get; set; } = default!;
    }
}
