using System;
using System.Collections.Generic;

namespace Model
{
    // клиент
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime Date { get; set; }

        public ICollection<CharacteristicValue> CharacteristicValues { get; set; }
        public ICollection<MetricValue> MetricValues { get; set; }
        public ICollection<ClientRatingValue> ClientRatingValue { get; set; }
    }
}