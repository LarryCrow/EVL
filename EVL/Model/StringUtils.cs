using System;
using System.Collections.Generic;
using System.Text;

namespace EVL.Model
{
    public static class QuestionPurposeNames
    {
        public const string Characteristic = "Свойство";
        public const string ClientRating = "Оценка клиента";
        public const string Segment = "Сегмент";
        public const string Unused = "Неиспользуемое";

        public static readonly string[] All = { Characteristic, ClientRating, Segment, Unused };
    }
}
