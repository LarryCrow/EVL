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
        public DbSet<ClientRatingValue> ClientRatingValues { get; set; }
        public DbSet<ClientRating> ClientRatings { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<CharacteristicValue> CharacteristicValues { get; set; }
        public DbSet<MetricToSegmentConditionalProbability> Probabilities { get; set; }

        public DbSet<Segment> Segments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Project> Projects { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Metric>().HasAlternateKey(m => new {m.Name, m.ProjectId});
            modelBuilder.Entity<ClientRating>().HasAlternateKey(cr => new {cr.Name, cr.ProjectId});
            modelBuilder.Entity<Characteristic>().HasAlternateKey(ch => new { ch.Name, ch.ProjectId });
            modelBuilder.Entity<Segment>().HasAlternateKey(s => new { s.Name, s.ProjectId });
            modelBuilder.Entity<Project>().HasAlternateKey(p => p.Name);
        }
    }
}