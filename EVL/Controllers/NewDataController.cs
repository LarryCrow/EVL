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
                viewState.metricQA.Add(new MetricQuestionAnswer {
                    QuestionId = m.Id,
                    QuestionName = m.Name,
                    QuestionDescription = m.Description,
                    QuestionPurposeName = QuestionPurposeNames.Metric,
                    Answers = answers
                });
            }

            foreach (Characteristic ch in characteristics)
            {
                viewState.characteristicQA.Add(new CharacteristicQuestionAnswer
                {
                    QuestionId = ch.Id,
                    QuestionName = ch.Name,
                    QuestionDescription = ch.Description,
                    QuestionPurposeName = QuestionPurposeNames.Characteristic
                });
            }

            foreach (ClientRating cr in clientsRating)
            {
                viewState.ratingQA.Add(new ClientRatingQuestionAnswer
                {
                    QuestionId = cr.Id,
                    QuestionName = cr.Name,
                    QuestionDescription = cr.Description,
                    QuestionWeight = cr.Weight,
                    QuestionPurposeName = QuestionPurposeNames.ClientRating
                });
            }
        }

        public void CalculateLoyalty()
        {
            var context = createDbContext();
            double perceptualLoyalty = CalculatePerceptualLoyalty();
            double[] conditionalProbabilities = new double[context.Segments.Where(s => s.Id == viewState.CurrentProjectID).Count()];
            for (int i = 0; i < conditionalProbabilities.Count(); i++)
            {
                conditionalProbabilities[i] = CalctulateConditionalProbability();
            }

        }

        public double CalculatePerceptualLoyalty()
        {
            double result = 0;
            foreach (ClientRatingQuestionAnswer cr in viewState.ratingQA)
            {
                result += cr.Answer * cr.QuestionWeight;
            }
            return (result / viewState.ratingQA.Count);
        }

        public double CalctulateConditionalProbability()
        {
            return 0;
        }
    }
}
