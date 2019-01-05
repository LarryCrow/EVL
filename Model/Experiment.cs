using System;
using System.Collections.Generic;

namespace Model
{
    public class Experiment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public ICollection<EQuestion> EQuestions { get; set; }
    }
}