using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVL.Model
{
    public class MetricQuestionAnswer
    {
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string QuestionDescription { get; set; }
        public string QuestionPurposeName { get; set; }
        
        public List<string> Answers { get; set; }
        public string SelectedAnswer { get; set; }
    }
}
