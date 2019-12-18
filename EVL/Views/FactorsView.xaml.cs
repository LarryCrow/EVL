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
        private readonly FactorsController controller;
        private readonly IReadOnlyFactorsViewState viewState;

        private (Result result, Question question) GetSelected()
            => (ResultsTable.SelectedItem as Result, QuestionsTable.SelectedItem as Question);

        public FactorsView(IReadOnlyFactorsViewState viewState, FactorsController controller)
        {
            this.controller = controller;
            this.viewState = viewState;
            InitializeComponent();

            QuestionsTable.ItemsSource = viewState.Questions;
            ResultsTable.ItemsSource = viewState.Results;
        }

        private void AddQuestion_Click(object source, RoutedEventArgs args) => controller.AddQuestion();

        private void AddResult_Click(object source, RoutedEventArgs args) => controller.AddResult();

        private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var (result, question) = GetSelected();
            WeightsTable.ItemsSource = viewState.GetWeights(result, question);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            controller.SubmitChanges();
        }
    }
}
