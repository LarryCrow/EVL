using EVL.Controllers;
using EVL.Model;
using Model.Entites;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EVL.Views
{
    public partial class FactorsView : UserControl
    {
        private readonly IReadOnlyFactorsViewState dataSource;
        private readonly FactorsController controller;

        private (IEnumerable<Result> results, IEnumerable<Question> questions) GetSelected()
            => (ResultsTable.SelectedItems?.OfType<Result>(), QuestionsTable.SelectedItems?.OfType<Question>());

        public FactorsView(IReadOnlyFactorsViewState viewState, FactorsController controller)
        {
            this.controller = controller;
            this.dataSource = viewState;
            InitializeComponent();
            ForceUpdateView();
        }

        private void AddQuestion_Click(object source, RoutedEventArgs args) =>
            controller.AddQuestionToState();

        private void AddResult_Click(object source, RoutedEventArgs args) =>
            controller.AddResultToState();

        private void AddWeight_Click(object sender, RoutedEventArgs e)
        {
            var (results, questions) = GetSelected();
            controller.AddWeight(results.Single(), questions.Single());

            WeightsTable.ItemsSource = dataSource.GetWeights(results, questions);
            AddWeightButton.Visibility = Visibility.Collapsed;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs args)
        {
            controller.SubmitChanges();
            ForceUpdateView();
            MessageBox.Show("Data synched");
        }

        private void Table_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var (result, question) = GetSelected();
            var weights = dataSource.GetWeights(result, question);

            WeightsTable.ItemsSource = weights;
            AddWeightButton.Visibility = result != null && question != null && !weights.Any() 
                ? Visibility.Visible 
                : Visibility.Collapsed;
        }

        private void ForceUpdateView()
        {
            QuestionsTable.ItemsSource = dataSource.Questions;
            ResultsTable.ItemsSource = dataSource.Results;
        }
    }
}
