using EVL.Utils;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EVL.Model
{
    public class ReadOnlyDatabaseViewState
    {
        public IEnumerable<CompanyUI> Companies { get; }
        private readonly ReadOnlyDictionary<CompanyUI, ReadOnlyCollection<QuestionAnswerUI>> blankets;

        public ReadOnlyDatabaseViewState(int projectId, DataBaseContext context)
        {
            this.blankets =
                (from c in context.Companies
                        .Where(c => c.Segment.ProjectId == projectId)
                        .Include(c => c.Segment)
                        .Include(c => c.MetricValueVotes)
                        .ThenInclude(mvv => mvv.MetricValue)
                        .ThenInclude(mv => mv.Metric)
                        .Include(c => c.ClientRatingValues)
                        .ThenInclude(crv => crv.ClientRating)
                        .Include(c => c.CharacteristicValues)
                        .ThenInclude(cv => cv.Characteristic)

                 select new
                 {
                     cui =
                         new CompanyUI
                         {
                             Name = c.Name,
                             Segment = c.Segment.Name,
                             Loyalty = c.Loyalty,
                             DateTime = c.Date
                         },

                     qas =
                         (from mvv in c.MetricValueVotes
                          select new QuestionAnswerUI
                          {
                              QuestionName = mvv.MetricValue.Metric.Name,
                              Answer = mvv.MetricValue.Value,
                              QuestionPurpose = QuestionPurposeNames.Metric
                          }).Concat
                         (from crv in c.ClientRatingValues
                          select new QuestionAnswerUI
                          {
                              QuestionName = crv.ClientRating.Name,
                              Answer = crv.Value.ToString(),
                              QuestionPurpose = QuestionPurposeNames.ClientRating
                          }).Concat
                         (from cv in c.CharacteristicValues
                          select new QuestionAnswerUI
                          {
                              QuestionName = cv.Characteristic.Name,
                              Answer = cv.Value,
                              QuestionPurpose = QuestionPurposeNames.Characteristic
                          }).ToList()
                         .AsReadOnly()
                 }).ToDictionary(o => o.cui, o => o.qas)
                .AsReadOnly();

            Companies = this.blankets.Keys;
        }

        public IEnumerable<QuestionAnswerUI> GetBlanket(CompanyUI company)
            => this.blankets[company];
    }
}
