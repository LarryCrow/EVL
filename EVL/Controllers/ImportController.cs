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
        public void ParseFile(string filename, string separator, int projectID)
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
                        ProjectId = projectID,
                        QuestionTypeName = fields[1],
                        QuestionViewName = fields[2],
                        QuestionPurposeName = fields[3]
                    };

                    //context.Questions.Add(q);
                    //context.SaveChanges();

                    viewState.AddQuestion(q);
                }
            }
        }

        public void ImportData(IEnumerable<QuestionUI> newQuestions, int projectID)
        {
            IEnumerable<string> FindIntersection<K>(IEnumerable<K> source1, IEnumerable<K> source2, Func<K, string> func)
                => source2.Select(func).Intersect(source1.Select(func));

            var untrackedSegments = new List<Segment>();
            var untrackedQuestions = new List<Question>();
            
            

            using (var context = createDbContext())
            {
                var qps = context.QuestionPurposes.ToDictionary(qp => qp.Name, qp => qp.Id);
                var qvs = context.QuestionViews.ToDictionary(qv => qv.Name, qv => qv.Id);
                var qts = context.QuestionTypes.ToDictionary(qt => qt.Name, qt => qt.Id);

                foreach (var q in newQuestions)
                {
                    switch (q.QuestionPurposeName)
                    {
                        case QuestionPurposeNames.Characteristic:
                        case QuestionPurposeNames.ClientRating:
                            untrackedQuestions.Add(new Question
                            {
                                Name = q.Name,
                                Weight = q.Weight,
                                ProjectId = q.ProjectId,
                                QuestionPurposeId = qps[q.QuestionPurposeName],
                                QuestionTypeId = qts[q.QuestionTypeName],
                                QuestionViewId = qvs[q.QuestionViewName]
                            });
                            break;
                        case QuestionPurposeNames.Segment:
                            untrackedSegments.Add(new Segment
                            {
                                Name = q.Name,
                                ProjectId = projectID
                            });
                            break;
                    }
                }

                try
                {
                    context.Segments.AddRange(untrackedSegments);
                    context.Questions.AddRange(untrackedQuestions);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var copies1 = FindIntersection(context.Questions, untrackedQuestions, q => q.Name).Select(str => $"Вопрос: {str}");
                    var copies2 = FindIntersection(context.Segments, untrackedSegments, s => s.Name).Select(str => $"Сегмент: {str}");

                    throw new InvalidOperationException(
                        $"Часть элементов уже существует в базе:\n{string.Join(",\n", copies1.Concat(copies2))}", ex);
                }
            }
        }
    }
}
