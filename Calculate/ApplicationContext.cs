using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Calculate
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Calc> Calcs { get; set; }
        public ApplicationContext() : base("DefaultConnection") { }
    }
}
