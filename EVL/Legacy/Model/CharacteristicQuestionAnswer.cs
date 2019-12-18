namespace EVL.Model
{
    public class CharacteristicQuestionAnswer
    {
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string QuestionDescription { get; set; }
        public string QuestionPurposeName { get; set; }

        public string Answer { get; set; }
    }
}
