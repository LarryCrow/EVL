using Microsoft.EntityFrameworkCore;

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
    }
}