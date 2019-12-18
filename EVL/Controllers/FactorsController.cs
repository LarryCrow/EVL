using System;
using System.Linq;
using EVL.Model;
using Model;

namespace EVL.Controllers
{
    public class FactorsController
    {
        private readonly FactorsViewState viewState;
        private readonly Func<EvlContext> createDbContext;

        public FactorsController(FactorsViewState viewState, Func<EvlContext> createDbContext)
        {
            this.viewState = viewState;
            this.createDbContext = createDbContext;
        }

        public void AddMetricValue(MetricUI metric, MetricValueUI mv)
        {
            viewState.AddMetricValue(metric, mv);
        }

        public void SubmitChanges()
        {
            using (var context = createDbContext())
            {
                var segmentPairs = Enumerable.Join(
                        context.Questions,
                        viewState.Segments,
                        s => s.Id,
                        s => s.Id,
                        (s1, s2) => new { dbs = s1, uis = s2 }
                    );

                foreach (var o in segmentPairs)
                {
                    o.dbs.Probability = o.uis.Probability;
                }
                
                var metricValues =
                    (from m in viewState.Metrics
                     from mv in viewState.GetMetricValues(m)
                     select new { m, mv })
                    .ToDictionary(
                        o => o.mv,
                        o => new MetricValue
                        {
                            MetricId = o.m.Id,
                            Value = o.mv.Value
                        }
                    );

                var oldMetricValues = context.MetricValues
                    .Where(p => p.Metric.ProjectId == viewState.ProjectId);

                var newProbabilities =
                    from metSegMvi in viewState.MetricValues
                    from segMvi in metSegMvi.Value
                    from mvi in segMvi.Value
                    select new MetricValueToSegmentConditionalProbability
                    {
                        SegmentId = segMvi.Key.Id,
                        ConditionalProbability = mvi.Probability,
                        MetricValue = metricValues[mvi.MetricValue]
                    };

                context.MetricValues.RemoveRange(oldMetricValues);
                context.Probabilities.AddRange(newProbabilities);
                context.SaveChanges();
            }
        }
    }
}
