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

                /*if (hasHeader == true && !parser.EndOfData)
                {
                    parser.ReadFields();
                }

                while (parser.LineNumber < startRow && !parser.EndOfData)
                    parser.ReadFields();*/

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    QuestionUI q = new QuestionUI
                    {
                        Name = fields[0],
                        QuestionPurposeName = fields[3]
                    };

                    //context.Questions.Add(q);
                    //context.SaveChanges();

                    viewState.AddQuestion(q);
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
                        case QuestionPurposeNames.ClientRating:
                            untrackedMetrics.Add(new Metric
                            {
                                Name = q.Name,
                                Weight = q.Weight.Value,
                                ProjectId = projectId,
                                Description = q.Description
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
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var copies1 = FindIntersection(context.Metrics, untrackedMetrics, q => q.Name)
                        .Select(str => $"{QuestionPurposeNames.ClientRating}: {str}");
                    var copies2 = FindIntersection(context.Segments, untrackedSegments, s => s.Name)
                        .Select(str => $"{QuestionPurposeNames.Segment}: {str}");
                    var copies3 = FindIntersection(context.Characteristics, untrackedCharacteristics, c => c.Name)
                        .Select(str => $"{QuestionPurposeNames.Characteristic}: {str}");

                    throw new InvalidOperationException(
                        "Часть элементов уже существует в базе:\n" +
                        string.Join(",\n", copies1.Concat(copies2).Concat(copies3)), ex);
                }
            }
        }
    }
}
