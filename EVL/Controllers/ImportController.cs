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

namespace EVL.Controllers
{
    public class ImportController
    {
        private ViewState viewState;
        private readonly DataBaseContext context;

        public ImportController(ViewState viewState, DataBaseContext context)
        {
            this.viewState = viewState ?? throw new ArgumentNullException("import model is null");
            this.context = context;
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
                    Question q = new Question
                    {
                        Name = fields[0],
                        ProjectId = projectID,
                        QuestionType = viewState.QuestionTypes[fields[1]],
                        QuestionView = viewState.QuestionViews[fields[2]],
                        QuestionPurpose = viewState.QuestionPurposes[fields[3]]
                    };

                    //context.Questions.Add(q);
                    //context.SaveChanges();

                    viewState.AddQuestion(q);
                }
            }
        }

        public void ImportData(IEnumerable<Question> newQuestions, int projectID)
        {
            string message((string name, string type) errCause) =>
                $"Элемент типа \'{errCause.type}\' c наименованием \'{errCause.name}\' уже существует в базе";
            
            foreach (var q in newQuestions)
            {
                (string name, string type) possibleErrorCause = ("", "");

                switch (q.QuestionPurpose.Name)
                {
                    case QuestionPurposeNames.Characteristic:
                    case QuestionPurposeNames.ClientRating:
                        possibleErrorCause = (q.Name, "Вопрос");

                        context.Questions.Add(q);
                        break;
                    case QuestionPurposeNames.Segment:
                        var segment = new Segment {Name = q.Name, ProjectId = projectID};
                        possibleErrorCause = (segment.Name, "Сегмент");

                        try
                        {
                            context.Segments.Add(segment);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(message(possibleErrorCause), ex);
                        }

                        break;
                }

                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    throw new InvalidOperationException(message(possibleErrorCause), ex);
                }
            }

            
        }
    }
}
