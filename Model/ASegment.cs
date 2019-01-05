namespace Model
{
    public class ASegment
    {
        public int Id { get; set; }

        public int SegmentId { get; set; }
        public Segment Segment { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}