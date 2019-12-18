using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class MetricValueVote
    {
        [Key]
        public int Id { get; set; }

        public int MetricValueId { get; set; }
        public MetricValue MetricValue { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}