using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vcssAPI.DBContext;

namespace vcssAPI
{
    public class apiDBContext : ctaDBContext
    {
        readonly string conexxionString = "Server=AMRO-HP;Database=ctaDB;Trusted_Connection=True;";        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(conexxionString);
            }
        }
    }
}
