using System.Collections.Generic;

namespace Model
{
    public class EQuestion
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int ExperimentId { get; set; }
        public Experiment Experiment { get; set; }
    }
}