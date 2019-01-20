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
                IEnumerable<MetricValue> mValues = m.MetricValues;
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
            var context = createDbContext();
            double perceptualLoyalty = CalculatePerceptualLoyalty();
            IEnumerable<Segment> segments = context.Segments.Where(s => s.Id == viewState.CurrentProjectID);
            double[] conditionalProbabilities = new double[segments.Count()];
            for (int i = 0; i < conditionalProbabilities.Count(); i++)
            {
                Segment s = segments.ElementAt(i);
                // TODO сделать после изменения модели
                IEnumerable<MetricValue> mv = null;
                conditionalProbabilities[i] = CalctulateConditionalProbability(segments.ElementAt(i), mv);
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

        private double CalctulateConditionalProbability(Segment segment, IEnumerable<MetricValue> metricValue)
        {
            
            return 0;
        }
    }
}
