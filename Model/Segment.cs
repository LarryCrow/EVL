using System.Collections.Generic;

namespace Model
{
    public class Segment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public double Probability { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<MetricValueToSegmentConditionalProbability> Probabilities { get; set; }
    }
}