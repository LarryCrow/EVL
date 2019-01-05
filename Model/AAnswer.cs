namespace Model
{
    public class AAnswer
    {
        public int Id { get; set; }

        public int AQuestionId { get; set; }
        public AQuestion AQuestion { get; set; }

        public int AnswerId { get; set; }
        public Answer Answer { get; set; }

        public string Value { get; set; }

    }
}