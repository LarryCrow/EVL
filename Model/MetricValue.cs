using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class MetricValue
    {
        [Key]
        public int Id { get; set; }
        public double Value { get; set; }

        public int MetricId { get; set; }
        public Metric Metric { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}