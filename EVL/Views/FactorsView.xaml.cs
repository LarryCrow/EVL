using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EVL.Model;
using EVL.Controllers;
using Model.Entites;
using Saritasa.Tools.Domain.Exceptions;

namespace EVL.Views
{
    public partial class FactorsView : UserControl
    {
        private readonly IReadOnlyFactorsViewState dataSource;
        private readonly FactorsController controller;

        private (IEnumerable<DeletableUI<Result>> results, IEnumerable<DeletableUI<Question>> questions, IEnumerable<DeletableUI<Weight>> weights) GetSelected()
            => (ResultsTable.SelectedItems?.OfType<DeletableUI<Result>>(),
                QuestionsTable.SelectedItems?.OfType<DeletableUI<Question>>(),
                WeightsTable.SelectedItems?.OfType<DeletableUI<Weight>>());

        public FactorsView(IReadOnlyFactorsViewState viewState, FactorsController controller)
        {
            this.controller = controller;
            this.dataSource = viewState;
            InitializeComponent();
            ForceUpdateView();
        }

        private void DeleteButton_Click(object source, RoutedEventArgs args)
        {
            var (results, questions, weights) = GetSelected();
            controller.ChangeEntitiesState(results, questions, weights, deleted: true);
            ClearSelection();
        }

        private void CancelDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var (results, questions, weights) = GetSelected();
            controller.ChangeEntitiesState(results, questions, weights, deleted: false);
            ClearSelection();
        }

        private void AddQuestion_Click(object source, RoutedEventArgs args) =>
            controller.AddQuestionToState();

        private void AddResult_Click(object source, RoutedEventArgs args) =>
            controller.AddResultToState();

        private void AddWeight_Click(object sender, RoutedEventArgs e)
        {
            var (results, questions, _) = GetSelected();
            controller.AddWeight(results.Single(), questions.Single());

            WeightsTable.ItemsSource = dataSource.GetWeights(results, questions);
            AddWeightButton.Visibility = Visibility.Collapsed;
        }

        private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var (results, questions, _) = GetSelected();
            var newWeights = dataSource.GetWeights(results.Any() ? results : null,
                                                   questions.Any() ? questions : null);

            WeightsTable.ItemsSource = newWeights;
            AddWeightButton.Visibility = results.Count() == 1 && questions.Count() == 1 && !newWeights.Any()
                ? Visibility.Visible
                : Visibility.Collapsed;

            WeightsTable_SelectionChanged(sender, e);
        }

        private void WeightsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var (results, questions, weights) = GetSelected();

            CancelDeleteButton.Visibility = results.Any(r => r.Deleted) || questions.Any(q => q.Deleted) || weights.Any(w => w.Deleted)
                ? Visibility.Visible
                : Visibility.Collapsed;

            DeleteButton.Visibility = results.Any() || questions.Any() || weights.Any()
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void ForceUpdateView()
        {
            QuestionsTable.ItemsSource = dataSource.Questions;
            ResultsTable.ItemsSource = dataSource.Results;
            WeightsTable.ItemsSource = dataSource.GetWeights();
        }

        private void ClearSelection()
        {
            ResultsTable.UnselectAll();
            QuestionsTable.UnselectAll();
            WeightsTable.UnselectAll();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs args)
        {
            try
            {
                controller.SubmitChanges();
                MessageBox.Show("Data synched");
            }
            catch (ValidationException ex)
            {
                var message = ex.Errors.Aggregate(ex.Message, (s, e) => s + $"\n{e.Key}:\n\t{string.Join("\n\t", e.Value)}");
                MessageBox.Show(Window.GetWindow(this), message);
            }
            finally
            {
                ForceUpdateView();
                ClearSelection();
            }
        }
    }
}
