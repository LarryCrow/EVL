using System.Collections.Generic;

namespace Model
{
    public class RAnswer
    {
        public int Id { get; set; }

        public int RQuestionId { get; set; }
        public RQuestion RQuestion { get; set; }

        public int AnswerId { get; set; }
        public Answer Answer { get; set; }

        public string Value { get; set; }

    }
}