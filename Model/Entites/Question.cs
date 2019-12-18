using System.ComponentModel.DataAnnotations;

namespace Model.Entites
{
    /// <summary>
    /// To be imported
    /// </summary>
    public class Question
    {
        [Key]
        public int Id { get; internal set; }

        public string Property { get; set; }

        public string QuestionText { get; set; }
    }
}
