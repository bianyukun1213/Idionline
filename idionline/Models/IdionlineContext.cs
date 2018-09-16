using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Idionline.Models
{
    public class IdionlineContext : DbContext
    {
        public IdionlineContext(DbContextOptions<IdionlineContext> options) : base(options)
        {

        }
        public DbSet<Idiom> Idioms { get; set; }
    }
}