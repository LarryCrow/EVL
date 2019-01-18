using System;
using System.Collections.Generic;

namespace Model
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ProjectDate { get; set; }
        public string Description { get; set; }

        public ICollection<Metric> Metrics { get; set; }
        public ICollection<Characteristic> Characteristics { get; set; }
    }
}