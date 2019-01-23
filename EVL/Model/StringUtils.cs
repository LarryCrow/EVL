namespace EVL.Model
{
    public static class QuestionPurposeNames
    {
        public const string Characteristic = "Название клиента";
        public const string Metric = "Свойство";
        public const string ClientRating = "Оценка клиента";
        public const string Segment = "Сегмент";
        public const string Unused = "Неиспользуемое";

        public static readonly string[] All = { Characteristic, Metric, ClientRating, Segment, Unused };
    }
}
