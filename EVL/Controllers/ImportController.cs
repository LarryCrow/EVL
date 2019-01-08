using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using EVL.Views;
using Microsoft.VisualBasic.FileIO;

namespace EVL.Controllers
{
    public class ImportController
    {
        private ApplicationModel _model;

        public ImportController(ApplicationModel importModel)
        {
            if (importModel == null)
                throw new ArgumentNullException("import model is null");
            _model = importModel;
        }

        public void ShowImportView(MainWindow mainWin)
        {
            mainWin.MainScope.Content = new DataImportView(_model, this);
        }

        public void ParseFile(string filename, string separator, int startRow, bool hasHeader, int projectID)
        {
            using (TextFieldParser parser = new TextFieldParser(filename))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(separator);

                if (hasHeader == true && !parser.EndOfData)
                {
                    parser.ReadFields();
                }

                while (parser.LineNumber < startRow && !parser.EndOfData)
                    parser.ReadFields();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    Question q = new Question();
                    q.Name = fields[0];
                    q.ProjectId = projectID;
                    q.QuestionType = _model.GetQuestionType(fields[1]);
                    q.QuestionTypeId = q.QuestionType.Id;
                    q.QuestionView = _model.GetQuestionView(fields[2]);
                    q.QuestionViewId = q.QuestionView.Id;
                    q.QuestionPurpose = _model.GetQuestionPurpose(fields[3]);
                    q.QuestionPurposeId = q.QuestionPurpose.Id;
                    _model.questions.Add(q);
                }
            }
        }

    }
}
