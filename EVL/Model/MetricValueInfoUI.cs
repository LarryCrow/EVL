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
