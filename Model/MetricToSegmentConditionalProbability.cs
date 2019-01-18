using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class MetricToSegmentConditionalProbability
    {
        public int Id { get; set; }

        public int MetricId { get; set; }
        public Metric Metric { get; set; }

        public int SegmentId { get; set; }
        public Segment Segment { get; set; }

        public double ConditionalProbability { get; set; }
    }
}
