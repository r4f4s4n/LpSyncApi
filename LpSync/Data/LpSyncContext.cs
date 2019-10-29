using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LpSync.Models;

namespace LpSync.Models
{
    public class LpSyncContext : DbContext
    {
        public LpSyncContext (DbContextOptions<LpSyncContext> options)
            : base(options)
        {
        }

        public DbSet<LpSync.Models.Usuaria> Usuaria { get; set; }

        public DbSet<LpSync.Models.Acceso> Acceso { get; set; }
    }
}
