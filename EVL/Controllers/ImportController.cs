using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Microsoft.VisualBasic.FileIO;
using EVL.Model;
using Model.Entites;

namespace EVL.Controllers
{
    public class ImportController
    {
        private readonly ViewState viewState;
        private readonly Func<EvlContext> createDbContext;

        public ImportController(ViewState viewState, Func<EvlContext> createDbContext)
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
                    var q = new QuestionUI
                    {
                        Name = fields[1],
                        QuestionText = fields[2],
                    };

                    viewState.Questions.Add(q);
                }
            }
        }

        public void ImportData(IEnumerable<QuestionUI> newQuestions)
        {
            IEnumerable<string> FindIntersection<K>(IEnumerable<K> source1, IEnumerable<K> source2, Func<K, string> func)
                => source2.Select(func).Intersect(source1.Select(func));

            using (var context = createDbContext())
            {
                var untrackedQuestions = newQuestions
                    .Select(q => new Question { Property = q.Name, QuestionText = q.QuestionText })
                    .ToList();

                try
                {
                    context.Questions.AddRange(untrackedQuestions);
                    context.SaveChanges();
                    viewState.Questions.Clear();
                }
                catch (Exception ex)
                {
                    var copies1 = FindIntersection(context.Questions, untrackedQuestions, q => q.Property)
                        .Select(str => $"{QuestionPurposeNames.ClientRating}: {str}");

                    throw new InvalidOperationException(
                        "Часть элементов уже существует в базе:\n" + string.Join(",\n", copies1), ex);
                }
            }
        }
    }
}
