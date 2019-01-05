using System.Collections.Generic;

namespace Model
{
    public class AQuestion
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<AAnswer> AAnswers { get; set; }
    }
}