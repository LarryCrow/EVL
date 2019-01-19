using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using EVL.Views;
using Microsoft.VisualBasic.FileIO;
using EVL.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace EVL.Controllers
{
    public class ImportController
    {
        private ViewState viewState;
        private readonly Func<DataBaseContext> createDbContext;

        public ImportController(ViewState viewState, Func<DataBaseContext> createDbContext)
        {
            this.viewState = viewState ?? throw new ArgumentNullException("import model is null");
            this.createDbContext = createDbContext;
        }

        // move to model
        public void ParseFile(string filename, string separator)
        {
            using (TextFieldParser parser = new TextFieldParser(filename))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(separator);

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    QuestionUI q = new QuestionUI
                    {
                        Name = fields[0],
                        QuestionPurposeName = fields[3]
                    };

                    viewState.questions.Add(q);
                }
            }
        }

        public void ImportData(int projectId, IEnumerable<QuestionUI> newQuestions)
        {
            IEnumerable<string> FindIntersection<K>(IEnumerable<K> source1, IEnumerable<K> source2, Func<K, string> func)
                => source2.Select(func).Intersect(source1.Select(func));

            var untrackedSegments = new List<Segment>();
            var untrackedMetrics = new List<Metric>();
            var untrackedCharacteristics = new List<Characteristic>();
            var untrackedRatings = new List<ClientRating>();

            using (var context = createDbContext())
            {
                foreach (var q in newQuestions)
                {
                    switch (q.QuestionPurposeName)
                    {
                        case QuestionPurposeNames.Characteristic:
                            untrackedCharacteristics.Add(new Characteristic
                            {
                                Name = q.Name,
                                ProjectId = projectId,
                                Description = q.Description
                            });
                            break;
                        case QuestionPurposeNames.Metric:
                            untrackedMetrics.Add(new Metric
                            {
                                Name = q.Name,
                                Weight = q.Weight.Value,
                                ProjectId = projectId,
                                Description = q.Description
                            });
                            break;

                        case QuestionPurposeNames.ClientRating:
                            untrackedRatings.Add(new ClientRating
                            {
                                Name = q.Name,
                                ProjectId = projectId,
                                Description = q.Description,
                                Weight = q.Weight.Value
                            });
                            break;
                        case QuestionPurposeNames.Segment:
                            untrackedSegments.Add(new Segment
                            {
                                Name = q.Name,
                                ProjectId = projectId,
                                Description = q.Description
                            });
                            break;
                    }
                }

                try
                {
                    context.Segments.AddRange(untrackedSegments);
                    context.Metrics.AddRange(untrackedMetrics);
                    context.Characteristics.AddRange(untrackedCharacteristics);
                    context.ClientRatings.AddRange(untrackedRatings);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var copies1 = FindIntersection(context.ClientRatings, untrackedRatings, q => q.Name)
                        .Select(str => $"{QuestionPurposeNames.ClientRating}: {str}");
                    var copies2 = FindIntersection(context.Segments, untrackedSegments, s => s.Name)
                        .Select(str => $"{QuestionPurposeNames.Segment}: {str}");
                    var copies3 = FindIntersection(context.Characteristics, untrackedCharacteristics, c => c.Name)
                        .Select(str => $"{QuestionPurposeNames.Characteristic}: {str}");
                    var copies4 = FindIntersection(context.Metrics, untrackedMetrics, q => q.Name)
                        .Select(str => $"{QuestionPurposeNames.Metric}: {str}");

                    throw new InvalidOperationException(
                        "Часть элементов уже существует в базе:\n" +
                        string.Join(",\n", copies1.Concat(copies2).Concat(copies3).Concat(copies4)), ex);
                }
            }
        }
    }
}
