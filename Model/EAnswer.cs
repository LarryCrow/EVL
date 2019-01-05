namespace Model
{
    public class EAnswer
    {
        public int Id { get; set; }

        public int EQuestionId { get; set; }
        public EQuestion EQuestion { get; set; }

        public int AnswerId { get; set; }
        public Answer Answer { get; set; }

        public string Value { get; set; }

    }
}