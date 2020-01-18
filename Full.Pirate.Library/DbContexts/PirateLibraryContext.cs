using Full.Pirate.Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Full.Pirate.Library.DbContexts
{
    public class PirateLibraryContext : DbContext
    {
        //readonly IConfiguration configuration;
        public PirateLibraryContext(DbContextOptions<PirateLibraryContext> options)
            :base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }


    }
}
