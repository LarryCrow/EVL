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

        public DbSet<Weight> Weights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Weight>().HasKey(w => new { w.ResultId, w.QuestionId });
        }
    }
}
