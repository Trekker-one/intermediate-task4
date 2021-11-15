using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Consummer_api.Models;

namespace Consummer_api.DBContext
{
    public class CustomMessageDbContext : DbContext
    {
        public CustomMessageDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CustomMessage> Message { get; set; }
    }
}
