using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public static class QuestionPurposeNames
    {
        public const string Characteristic = "Свойство";
        public const string ClientRating = "Оценка клиента";
        public const string Segment = "Сегмент";
        public const string Unused = "Неиспользуемое";

        public static readonly string[] All = { Characteristic, ClientRating, Segment, Unused };
    }

    public static class QuestionTypeNames
    {
        public const string Real = "Вещественный";
        public const string String = "Строковый";
        public const string DateTime = "Дата/Время";

        public static readonly string[] All = { Real, String, DateTime };
    }

    public static class QuestionViewNames
    {
        public const string Continious = "Непрерывный";

        public static readonly string[] All = { Continious };
    }
}
