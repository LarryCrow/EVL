using EVL.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVL.Controllers
{
    public class NewDataController
    {
        private readonly ViewState viewState;
        private readonly Func<DataBaseContext> createDbContext;

        public NewDataController(ViewState viewState, Func<DataBaseContext> createDbContext)
        {
            this.viewState = viewState;
            this.createDbContext = createDbContext;
        }

        public void FillTable()
        {
            var context = createDbContext();
            int projectId = viewState.CurrentProjectID;
            IEnumerable<Metric> metrics = context.Metrics.Where(m => m.ProjectId == projectId);
            IEnumerable<Characteristic> characteristics = context.Characteristics.Where(c => c.ProjectId == projectId);
            IEnumerable<ClientRating> clientsRating = context.ClientRatings.Where(cr => cr.ProjectId == projectId);

            foreach(Metric m in metrics)
            {
                IEnumerable<MetricValue> mValues = context.MetricValues.Where(mv => mv.MetricId == m.Id);
                List<string> answers = new List<string>();
                if (mValues != null)
                {
                    foreach (MetricValue mv in mValues)
                    {
                        answers.Add(mv.Value.ToString());
                    }
                }
                viewState.MetricQA.Add(new MetricQuestionAnswer {
                    QuestionId = m.Id,
                    QuestionName = m.Name,
                    QuestionDescription = m.Description,
                    QuestionPurposeName = QuestionPurposeNames.Metric,
                    Answers = answers
                });
            }

            foreach (Characteristic ch in characteristics)
            {
                viewState.CharacteristicQA.Add(new CharacteristicQuestionAnswer
                {
                    QuestionId = ch.Id,
                    QuestionName = ch.Name,
                    QuestionDescription = ch.Description,
                    QuestionPurposeName = QuestionPurposeNames.Characteristic
                });
            }

            foreach (ClientRating cr in clientsRating)
            {
                viewState.RatingQA.Add(new ClientRatingQuestionAnswer
                {
                    QuestionId = cr.Id,
                    QuestionName = cr.Name,
                    QuestionDescription = cr.Description,
                    QuestionWeight = cr.Weight,
                    QuestionPurposeName = QuestionPurposeNames.ClientRating
                });
            }
        }

        public void AddToDataBase(Company c)
        {
            using(var context = createDbContext())
            {
                context.Companies.Add(c);
                context.SaveChanges();
            }
        }

        public void CalculateLoyalty()
        {
            using (var context = createDbContext())
            {
                double perceptualLoyalty = CalculatePerceptualLoyalty();

                IEnumerable<Segment> segments = context.Segments.Where(s => s.ProjectId == viewState.CurrentProjectID);

                double[] charProbabilityInSegment = CalctulateCharacteristicProbabilitiesInSegment(segments);

                double fullProbability = GetFullProbability(segments, charProbabilityInSegment);
                double[] conditionalProbabilities = new double[charProbabilityInSegment.Count() - 1];
                for (int i = 0; i < conditionalProbabilities.Count(); i++)
                {
                    Segment s = segments.ElementAt(i);
                    conditionalProbabilities[i] = (charProbabilityInSegment[i] * s.Probability) / fullProbability;
                }
            }       
        }

        private double CalculatePerceptualLoyalty()
        {
            double result = 0;
            foreach (ClientRatingQuestionAnswer cr in viewState.RatingQA)
            {
                result += cr.Answer * cr.QuestionWeight;
            }
            return (result / viewState.RatingQA.Count);
        }

        private double GetFullProbability(IEnumerable<Segment> segments, double[] charProbabilityInSegment)
        {
            double fullProbability = 0;
            for (int i = 0; i < charProbabilityInSegment.Count(); i++)
            {
                Segment s = segments.ElementAt(i);
                fullProbability += charProbabilityInSegment[i] * s.Probability;
            }
            return fullProbability;
        }


        private double[] CalctulateCharacteristicProbabilitiesInSegment(IEnumerable<Segment> segments)
        {
            double[] result = new double[segments.Count()];
            int index = 0;
            foreach (Segment s in segments)
            {
                var context = createDbContext();
                IEnumerable<MetricQuestionAnswer> mqa = viewState.MetricQA;
                List<MetricValue> mv = new List<MetricValue>();
                foreach (MetricQuestionAnswer m in mqa)
                {
                    mv.Add(context.MetricValues.Where(metricV => (metricV.MetricId == m.QuestionId && metricV.Value == m.SelectedAnswer)).First());
                }
                foreach (MetricValue m in mv)
                {
                    double mvtsProb = context.Probabilities.Where(metricVTS => (m.Id == metricVTS.MetricValueId && metricVTS.SegmentId == s.Id)).First().ConditionalProbability;
                    result[index] *= mvtsProb;
                }
                result[index] /= Math.Pow(s.Probability, mqa.Count() - 1);
                index++;
            }

            return result;
        }
    }
}
