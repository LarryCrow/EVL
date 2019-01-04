using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class Cat
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class AnimalDbContext : DbContext
    {
        public DbSet<Cat> Cats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite("Data Source=mydb.db;");
    }
}