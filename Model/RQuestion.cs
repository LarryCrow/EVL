using System.Collections.Generic;

namespace Model
{
    public class RQuestion
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public ICollection<RAnswer> RAnswers { get; set; }
    }
}