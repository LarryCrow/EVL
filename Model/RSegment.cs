namespace Model
{
    public class RSegment
    {
        public int Id { get; set; }

        public int SegmentId { get; set; }
        public Segment Segment { get; set; }

        public string Value { get; set; }
    }
}