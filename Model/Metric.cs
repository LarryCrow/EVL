using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    // свойство
    public class Metric
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public double Weight { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<MetricValue> MetricValues { get; set; }
    }
}
