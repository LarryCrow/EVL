using System;

namespace EVL.Model
{
    public class CompanyUI
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public string Segment { get; set; }
        public double Loyalty { get; set; }
    }

    public class QuestionAnswerUI
    {
        public string QuestionName { get; set; }
        public string QuestionPurpose { get; set; }
        public string Answer { get; set; }
    }
}
