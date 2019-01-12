using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using EVL.Views;
using Microsoft.VisualBasic.FileIO;
using EVL.Model;

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

                    context.Questions.Add(q);
                    context.SaveChanges();

                    viewState.AddQuestion(q);
                }
            }
        }

        public void ImportData(bool hasHeader, int startRow, int projectID)
        {
            // Запись сегмента
            viewState.GetSegments();

            // Запись вопросов (посмотри метод внутри, я там по айдишникам сравнивал, может что-то нужно изменить)
            context.Questions.AddRange(viewState.GetQuestions());

            // Это я пока не разобрался для чего
            // Просто в листинге было. Думаю бесполезный кусок
            // int[] clientsIndex = viewState.GetClientsIndex();
        }

    }
}
