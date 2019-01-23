using EVL.Controllers;
using EVL.Model;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EVL.Views
{
    /// <summary>
    /// Interaction logic for NewDataView.xaml
    /// </summary>
    public partial class NewDataView : UserControl
    {
        private readonly NewDataController controller;
        private readonly IReadOnlyNewDataViewState viewState;

        private readonly string[] dateFormats = new[] { "dd.MM.yyyy", "dd/MM/yyyy" };

        public NewDataView(IReadOnlyNewDataViewState viewState, NewDataController controller)
        {
            InitializeComponent();

            this.controller = controller;
            this.viewState = viewState;
            
            this.controller.FillTable();
            
            MetricsTable.ItemsSource = this.viewState.MetricQA;
            CharacteristicTable.ItemsSource = this.viewState.CharacteristicQA;
            RatingsTable.ItemsSource = this.viewState.ClientRatingQA;
        }

        private void AddToDB_Click(object sender, RoutedEventArgs e)
        {
            if (viewState.ClientLoyalty == -1)
            {
                MessageBox.Show("Сначала требуется рассчитать лояльность");
                return;
            }
            if (NameInput.Text == "")
            {
                MessageBox.Show("Заполните поле 'Наименование'");
            }
            var (culture, dtstyle) = (CultureInfo.CurrentUICulture, DateTimeStyles.None);

            if (DateTime.TryParseExact(DatePicker.Text, dateFormats, culture, dtstyle, out DateTime date))
            {
                try
                {
                    controller.AddToDataBase(NameInput.Text, date);
                    MessageBox.Show("Добавление прошло успешно");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("В базе уже имеется элемент с таким наименованием");
                }
            }
            else
            {
                MessageBox.Show("Введите дату в корректном формате.\nНапример: "
                                + string.Join(" или ", dateFormats));
            }
        }

        private void CalculateLoyalty_Click(object sender, RoutedEventArgs e)
        {
            bool metricsValue = viewState.MetricQA.All(m => m.SelectedAnswer != null);
            bool clientRatingsValue = viewState.ClientRatingQA.All(cr => cr.Answer > 0 && cr.Answer <= 10);
            bool characteristicsValue = viewState.CharacteristicQA.All(ch => ch.Answer != null);

            if (metricsValue == false || clientRatingsValue == false || characteristicsValue == false)
            {
                string message = "Пожалуйста, ответьте на все вопросы в таблицах:" +
                    (metricsValue == false ? "\nХарактеристика" : "") +
                    (clientRatingsValue == false ? "\nОценка клиента(в диапазоне от 1 до 10)" : "") +
                    (characteristicsValue == false ? "\nИнформация о клиенте" : "");

                MessageBox.Show(message);
            }
            else
            {
                controller.CalculateLoyalty();
                MessageBox.Show("Расчёт проивзедён успешно. Лояльность клиента " + viewState.ClientLoyalty);
            }

        }
    }
}
