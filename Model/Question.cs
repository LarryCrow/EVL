using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }

        public int QuestionViewId { get; set; }
        public QuestionView QuestionView { get; set; }

        public int QuestionPurposeId { get; set; }
        public QuestionPurpose QuestionPurpose { get; set; }
    }
}
