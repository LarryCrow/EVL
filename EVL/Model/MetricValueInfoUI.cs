using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVL.Model
{
    public class MetricValueUI
    {
        public string Value { get; set; }
    }

    public class MetricValueInfoUI
    {
        public MetricValueUI MetricValue { get; set; }
        public double Probability { get; set; }
    }
}
