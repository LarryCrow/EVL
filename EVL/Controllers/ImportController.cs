using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using EVL.Views;

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

    }
}
