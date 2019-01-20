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
        public ICollection<MetricValueVote> MetricValueVotes { get; set; }
        public ICollection<ClientRatingValue> ClientRatingValues { get; set; }
    }
}