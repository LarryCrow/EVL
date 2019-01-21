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
        private readonly NewDataViewState viewState;
        private readonly Func<DataBaseContext> createDbContext;

        public NewDataController(NewDataViewState viewState, Func<DataBaseContext> createDbContext)
        {
            this.viewState = viewState;
            this.createDbContext = createDbContext;
        }

        public void FillTable()
        {
            var context = createDbContext();
            int projectId = viewState.ProjectID;
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

        public void AddToDataBase(String name, DateTime date)
        {
            using (var context = createDbContext())
            {
                Company c = new Company()
                {
                    Name = name,
                    Date = date,
                    Loyalty = viewState.ClientLoyalty,
                    PriorLoyalty = viewState.PriorClientLoyalty,
                    SegmentId = viewState.SegmentID
                };

                List<MetricValueVote> mv = new List<MetricValueVote>();
                foreach (MetricQuestionAnswer metricQA in viewState.MetricQA)
                {
                    mv.Add(new MetricValueVote()
                    {
                        MetricValue = context.MetricValues.Where(metricV => metricV.Value == metricQA.SelectedAnswer &&
                                                                            metricV.MetricId == metricQA.QuestionId).First(),     
                        Company = c
                    });
                }

                List<ClientRatingValue> crv = new List<ClientRatingValue>();
                foreach (ClientRatingQuestionAnswer ratingQA in viewState.RatingQA)
                {
                    crv.Add(new ClientRatingValue()
                    {
                        Value = ratingQA.Answer,
                        ClientRatingId = ratingQA.QuestionId,
                        Company = c
                    });
                }

                List<CharacteristicValue> cv = new List<CharacteristicValue>();
                foreach (CharacteristicQuestionAnswer characteristicQA in viewState.CharacteristicQA)
                {
                    cv.Add(new CharacteristicValue()
                    {
                        Value = characteristicQA.Answer,
                        CharacteristicId = characteristicQA.QuestionId,
                        Company = c
                    });
                }

                c.MetricValueVotes = mv;
                c.ClientRatingValues = crv;
                c.CharacteristicValues = cv;
                
                context.Companies.Add(c);
                context.SaveChanges();
            }
        }

        public void CalculateLoyalty()
        {
            using (var context = createDbContext())
            {
                double perceptualLoyalty = CalculatePerceptualLoyalty();

                IEnumerable<Segment> segments = context.Segments.Where(s => s.ProjectId == viewState.ProjectID);

                double[] charProbabilityInSegment = CalctulateCharacteristicProbabilitiesInSegment(segments);

                double fullProbability = GetFullProbability(segments, charProbabilityInSegment);

                double clientLoyalty = 0;
                double maxProbSegment = 0;
                int segmentID = 0;
                for (int i = 0; i < segments.Count(); i++)
                {
                    Segment s = segments.ElementAt(i);
                    double conditionalProbabilitiesI = (charProbabilityInSegment[i] * s.Probability) / fullProbability;
                    double segmentsLoyaltyI = CalculateSegmentLoyalty(s);
                    clientLoyalty += conditionalProbabilitiesI * segmentsLoyaltyI;

                    if (conditionalProbabilitiesI > maxProbSegment)
                    {
                        segmentID = s.Id;
                        maxProbSegment = conditionalProbabilitiesI;
                    }
                }

                viewState.PriorClientLoyalty = perceptualLoyalty;
                viewState.ClientLoyalty = clientLoyalty;
                viewState.SegmentID = segmentID;
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
            using(var context = createDbContext())
            {
                double[] result = Enumerable.Repeat((double)1, segments.Count()).ToArray();
                int index = 0;
                IEnumerable<MetricQuestionAnswer> mqa = viewState.MetricQA;
                List<MetricValue> mv = new List<MetricValue>();
                foreach (MetricQuestionAnswer m in mqa)
                {
                    mv.Add(context.MetricValues.Where(metricV => (metricV.MetricId == m.QuestionId && metricV.Value == m.SelectedAnswer)).First());
                }
                foreach (Segment s in segments)
                {
                    foreach (MetricValue m in mv)
                    {
                        double mvtsProb = context.Probabilities.Where(metricVTS => (m.Id == metricVTS.MetricValueId && metricVTS.SegmentId == s.Id)).First().ConditionalProbability;
                        result[index] *= mvtsProb;
                    }
                    index++;
                }
                return result;
            }
        }

        private double CalculateSegmentLoyalty(Segment s)
        {
            double result = 0;
            using(var context = createDbContext())
            {
                IEnumerable<Company> companies = context.Companies.Where(c => c.SegmentId == s.Id); 
                if (companies.Count() != 0)
                {
                    foreach (Company c in companies)
                    {
                        result += c.PriorLoyalty;
                    }
                    result /= companies.Count();
                }
                else
                {
                    result = 1;
                }
            }
            return result;
        }
    }
}
