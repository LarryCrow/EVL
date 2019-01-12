using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
