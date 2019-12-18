using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Entites;

namespace Model
{
    public class EvlContext : DbContext
    {
        public EvlContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Result> Results { get; set; }
    }
}
