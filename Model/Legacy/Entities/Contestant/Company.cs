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

        public Segment Segment { get; set; }
        public int SegmentId { get; set; }

        public double PriorLoyalty { get; set; }
        public double Loyalty { get; set; }

        public int ProjectID { get; set; }

        public ICollection<CharacteristicValue> CharacteristicValues { get; set; }
        public ICollection<MetricValueVote> MetricValueVotes { get; set; }
        public ICollection<ClientRatingValue> ClientRatingValues { get; set; }
    }
}