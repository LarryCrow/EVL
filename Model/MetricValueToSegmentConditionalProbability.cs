using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class MetricValueToSegmentConditionalProbability
    {
        public int Id { get; set; }

        public int MetricValueId { get; set; }
        public MetricValue MetricValue { get; set; }

        public int SegmentId { get; set; }
        public Segment Segment { get; set; }

        public double ConditionalProbability { get; set; }
    }
}
