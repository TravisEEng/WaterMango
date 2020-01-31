using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Assignment.Models
{
    public class PlantDb : DbContext
    {
        public DbSet<Plant> Plants { get; set; }
    
    }
}