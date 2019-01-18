using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Model
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<MetricValue> MetricValues { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<CharacteristicValue> CharacteristicValues { get; set; }
        public DbSet<MetricToSegmentConditionalProbability> Probabilities { get; set; }

        public DbSet<Segment> Segments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Project> Projects { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Metric>().HasAlternateKey(q => q.Name);
            modelBuilder.Entity<Characteristic>().HasAlternateKey(q => q.Name);
            modelBuilder.Entity<Segment>().HasAlternateKey(s => s.Name);
            modelBuilder.Entity<Project>().HasAlternateKey(p => p.Name);
        }
    }
}