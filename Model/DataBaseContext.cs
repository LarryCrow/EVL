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

        public DbSet<Answer> Answers { get; set; }
        public DbSet<AAnswer> AAnswers { get; set; }
        public DbSet<RAnswer> RAnswers { get; set; }
        public DbSet<EAnswer> EAnswers { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<AQuestion> AQuestions { get; set; }
        public DbSet<RQuestion> RQuestions { get; set; }
        public DbSet<EQuestion> EQuestions { get; set; }

        public DbSet<Segment> Segments { get; set; }
        public DbSet<ASegment> ASegments { get; set; }
        public DbSet<RSegment> RSegments { get; set; }
        public DbSet<ESegment> ESegments { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<QuestionView> QuestionViews { get; set; }
        public DbSet<QuestionPurpose> QuestionPurposes { get; set; }

        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>().HasAlternateKey(q => q.Name);
            modelBuilder.Entity<Segment>().HasAlternateKey(s => s.Name);

            modelBuilder.Entity<QuestionType>()
                .HasData(QuestionTypeNames.All.Select((qt, i) => new QuestionType {Id = i+1, Name = qt}));
            modelBuilder.Entity<QuestionView>()
                .HasData(QuestionViewNames.All.Select((qv, i) => new QuestionView {Id = i+1, Name = qv}));
            modelBuilder.Entity<QuestionPurpose>()
                .HasData(QuestionPurposeNames.All.Select((qp, i) => new QuestionPurpose {Id = i+1, Name = qp}));
        }
    }
}