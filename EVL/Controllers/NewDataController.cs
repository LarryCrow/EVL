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
                IEnumerable<MetricValue> values = context.MetricValues.Where(mv => mv.MetricId == m.Id);
                Dictionary<int, double> d = new Dictionary<int, double>();
                foreach(MetricValue mv in values)
                {
                    d.Add(mv.Id, mv.Value);
                }
                viewState.questionAnswers.Add(new QuestionAnswers {
                    QuestionId = m.Id,
                    QuestionName = m.Name,
                    QuestionDescription = m.Description,
                    QuestionPurposeName = QuestionPurposeNames.Metric,
                    Answers = new Dictionary<int, double>(d)
                });
            }
        }
    }
}
