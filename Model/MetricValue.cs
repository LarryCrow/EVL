using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    // свойство
    public class MetricValue
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }

        public int MetricId { get; set; }
        public Metric Metric { get; set; }

        public ICollection<MetricValueToSegmentConditionalProbability> Probabilities { get; set; }
    }
}