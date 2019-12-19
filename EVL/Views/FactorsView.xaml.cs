using EVL.Controllers;
using EVL.Model;
using Model.Entites;
using System.Windows;
using System.Windows.Controls;

namespace EVL.Views
{
    /// <summary>
    /// Логика взаимодействия для FactorsView.xaml
    /// </summary>
    public partial class FactorsView : UserControl
    {
        private readonly IReadOnlyFactorsViewState dataSource;
        private readonly FactorsController controller;

        private (Result result, Question question) GetSelected()
            => (ResultsTable.SelectedItem as Result, QuestionsTable.SelectedItem as Question);

        public FactorsView(IReadOnlyFactorsViewState viewState, FactorsController controller)
        {
            this.controller = controller;
            this.dataSource = viewState;
            InitializeComponent();

            QuestionsTable.ItemsSource = viewState.Questions;
            ResultsTable.ItemsSource = viewState.Results;
        }

        private void AddQuestion_Click(object source, RoutedEventArgs args) =>
            controller.AddQuestionToState();

        private void AddResult_Click(object source, RoutedEventArgs args) =>
            controller.AddResultToState();

        private void SaveButton_Click(object sender, RoutedEventArgs args) =>
            controller.SubmitChanges();

        private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var (result, question) = GetSelected();
            WeightsTable.ItemsSource = dataSource.GetWeights(result, question);
        }

    }
}
