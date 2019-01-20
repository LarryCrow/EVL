using EVL.Controllers;
using EVL.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EVL.Views
{
    /// <summary>
    /// Interaction logic for NewDataView.xaml
    /// </summary>
    public partial class NewDataView : UserControl
    {
        private readonly NewDataController controller;
        private readonly IReadOnlyViewState viewState;

        private readonly string[] dateFormats = new[] { "dd.MM.yyyy", "dd/MM/yyyy" };

        public NewDataView(IReadOnlyViewState viewState, NewDataController controller)
        {
            InitializeComponent();

            this.controller = controller;
            this.viewState = viewState;
            MetricsTable.ItemsSource = this.viewState.MetricQA;
            CharacteristicTable.ItemsSource = this.viewState.CharacteristicQA;
            RatingsTable.ItemsSource = this.viewState.ClientRatingQA;

            this.controller.FillTable();
        }

        public void AddToDataBase()
        {
            var (culture, dtstyle) = (CultureInfo.CurrentUICulture, DateTimeStyles.None);

            if (DateTime.TryParseExact(DatePicker.Text, dateFormats, culture, dtstyle, out DateTime date))
            {
                Company c = new Company()
                {
                    Name = NameInput.Text,
                    Date = date
                };

                List<MetricValue> mv = new List<MetricValue>();
                foreach (MetricQuestionAnswer metricQA in viewState.MetricQA)
                {
                    mv.Add(new MetricValue()
                    {
                        Value = Convert.ToInt32(metricQA.SelectedAnswer),
                        MetricId = metricQA.QuestionId,
                        // TODO CompanyID?
                    });
                }

                List<ClientRatingValue> crv = new List<ClientRatingValue>();
                foreach (ClientRatingQuestionAnswer ratingQA in viewState.ClientRatingQA)
                {
                    crv.Add(new ClientRatingValue()
                    {
                        Value = ratingQA.Answer,
                        ClientRatingId = ratingQA.QuestionId,
                        // TODO CompanyID?

                    });
                }

                List<CharacteristicValue> cv = new List<CharacteristicValue>();
                foreach (CharacteristicQuestionAnswer characteristicQA in viewState.CharacteristicQA)
                {
                    cv.Add(new CharacteristicValue()
                    {
                        Value = characteristicQA.Answer,
                        CharacteristicId = characteristicQA.QuestionId,
                        // TODO CompanyID?
                    });
                }

                c.MetricValues = mv;
                c.ClientRatingValue = crv;
                c.CharacteristicValues = cv;

                controller.AddToDataBase(c);
            } else
            {
                MessageBox.Show("Введите дату в корректном формате.\nНапример: "
                                + string.Join(" или ", dateFormats));
            }
        }

        public void CalculateLoyalty()
        {
            controller.CalculateLoyalty();
        }
    }
}
