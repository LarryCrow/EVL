namespace Model.Entites
{
    public class Weight
    {
        public double PPlus { get; set; }

        public double PMinus { get; set; }

        public int QuestionId { get; set; }

        public int ResultId { get; set; }

        public Result Result { get; set; }

        public Question Question { get; set; }
    }
}
