using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EVL.Model;
using EVL.Controllers;
using System;
using evl.Model;

namespace EVL.Views
{
    /// <summary>
    /// Логика взаимодействия для FactorsView.xaml
    /// </summary>
    public partial class FactorsView : UserControl
    {
        private readonly FactorsController controller;
        private readonly IReadOnlyFactorsViewState viewState;

        private (ResultUI metric, QuestionUI segment) GetSelected()
            => (MetricsTable.SelectedItem as MetricUI, SegmentsTable.SelectedItem as SegmentUI);

        public FactorsView(IReadOnlyFactorsViewState viewState, FactorsController controller)
        {
            InitializeComponent();

            this.controller = controller;
            this.viewState = viewState;
            MetricsTable.ItemsSource = viewState.Questions;
            SegmentsTable.ItemsSource = viewState.Segments;
        }

        private void AddRow_Click(object source, RoutedEventArgs args)
        {
            var (metric, segment) = GetSelected();
            if (metric != null && segment != null)
            {
                controller.AddMetricValue(metric, new MetricValueUI());
            }
            else
            {
                MessageBox.Show($"Выберите {QuestionPurposeNames.Segment} и {QuestionPurposeNames.Metric}");
            }
        }

        private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var (metric, segment) = GetSelected();
            if (metric != null && segment != null)
            {
                AnswersTable.ItemsSource = viewState.GetMetricValueInfos(metric, segment);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var emptyBlanks =
                (from m in viewState.Metrics
                 where !viewState.GetMetricValues(m).Any()
                 select m.Name).ToList();

            var invalidNames =
                (from m in viewState.Metrics
                 from o in viewState.GetMetricValues(m).Select((mv, i) => new { mv, i })
                 where string.IsNullOrEmpty(o.mv.Value)
                 select $"{m.Name}[{o.i}]").ToList();

            var invalidProbabilities =
                (from m in viewState.Metrics
                 from s in viewState.Segments
                 where viewState.GetMetricValueInfos(m, s).Sum(mvi => mvi.Probability) != 1
                 select $"{m.Name} :: {s.Name}").ToList();

            var isValidSegmentProbabilities = viewState.Segments.Sum(s => s.Probability) == 1;
            var isValidBlanks = !emptyBlanks.Any();
            var isValidNames = !invalidNames.Any();
            var isValidProbabilities = !invalidProbabilities.Any();

            if (isValidProbabilities && isValidBlanks && isValidSegmentProbabilities && isValidNames)
            {
                try
                {
                    controller.SubmitChanges();
                    MessageBox.Show("Данные импортированы");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                var message =
                    (!isValidSegmentProbabilities ? "Сумма вероятностей сегментов != 1" : "")
                    + (!isValidBlanks ? "\nПустые таблицы: \n\t" + string.Join("\n\t", emptyBlanks) : "")
                    + (!isValidNames ? "\nПустые ответы: \n\t" + string.Join("\n\t", invalidNames) : "")
                    + (!isValidProbabilities ? "\nСуммы вероятностей: \n\t" + string.Join("\n\t", invalidProbabilities) : "");

                MessageBox.Show(message);
            }
        }
    }
}
