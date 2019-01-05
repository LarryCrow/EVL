namespace Model
{
    public class ESegment
    {
        public int Id { get; set; }

        public int SegmentId { get; set; }
        public Segment Segment { get; set; }

        public int ExperimentId { get; set; }
        public Experiment Experiment { get; set; }

        public string Value { get; set; }
    }
}