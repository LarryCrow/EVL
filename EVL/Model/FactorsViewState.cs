using EVL.Utils;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EVL.Model
{
    public interface IReadOnlyFactorsViewState
    {
        IReadOnlyCollection<SegmentUI> Segments { get; }
        IReadOnlyCollection<MetricUI> Metrics { get; }
        IEnumerable<MetricValueUI> GetMetricValues(MetricUI metric);
        ReadOnlyObservableCollection<MetricValueInfoUI> GetMetricValueInfos(MetricUI metric, SegmentUI segment);
    }

    public class FactorsViewState : IReadOnlyFactorsViewState
    {
        public ReadOnlyDictionary<MetricUI,
            ReadOnlyDictionary<SegmentUI, ObservableCollection<MetricValueInfoUI>>> MetricValues
        { get; }

        public IReadOnlyCollection<SegmentUI> Segments { get; }

        public int ProjectId { get; }

        public FactorsViewState(int projectId, EvlContext context)
        {
            ProjectId = projectId;

            Segments = context.Segments
                .Where(s => s.ProjectId == projectId)
                .Select(s => new SegmentUI
                {
                    Id = s.Id,
                    Name = s.Name,
                    Probability = s.Probability
                })
                .ToList();

            MetricValues = context.Metrics
                .Where(m => m.ProjectId == projectId)
                .Include(m => m.MetricValues)
                    .ThenInclude(mv => mv.Probabilities)
                .Select(m =>
                    new
                    {
                        mui = new MetricUI { Id = m.Id, Name = m.Name },
                        mvs = m.MetricValues
                            .Select(mv => new
                            {
                                id = mv.Id,
                                ui = new MetricValueUI { Value = mv.Value },
                                ps = mv.Probabilities.ToDictionary(p => p.SegmentId, p => p.ConditionalProbability)
                            })
                            .ToList()
                    }
                ).ToDictionary(
                    o => o.mui,
                    o => Segments.ToDictionary(s => s, s =>
                        new ObservableCollection<MetricValueInfoUI>(
                            o.mvs.Select(mv => new MetricValueInfoUI
                            {
                                MetricValue = mv.ui,
                                Probability = mv.ps[s.Id]
                            })
                        )
                    ).AsReadOnly()
                ).AsReadOnly();
        }

        public IReadOnlyCollection<MetricUI> Metrics
            => MetricValues.Keys;

        public void AddMetricValue(MetricUI metric, MetricValueUI metricValue)
        {
            foreach (var item in MetricValues[metric])
            {
                item.Value.Add(new MetricValueInfoUI { MetricValue = metricValue });
            }
        }

        public ReadOnlyObservableCollection<MetricValueInfoUI> GetMetricValueInfos(MetricUI metric, SegmentUI segment)
            => MetricValues[metric][segment].AsReadOnly();


        public IEnumerable<MetricValueUI> GetMetricValues(MetricUI metric)
            => MetricValues[metric].First().Value.Select(mvi => mvi.MetricValue);
    }
}