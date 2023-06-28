using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Machine
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
 
        public ApplicationDbContext()
        {
                
        }
    }
}