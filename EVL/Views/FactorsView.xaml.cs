using evl.Model;
using EVL.Controllers;
using EVL.Model;
using Model.Entites;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EVL.Views
{
    public partial class FactorsView : UserControl
    {
        private readonly IReadOnlyFactorsViewState dataSource;
        private readonly FactorsController controller;

        private (IEnumerable<DeletableUI<Result>> results, IEnumerable<DeletableUI<Question>> questions) GetSelected()
            => (ResultsTable.SelectedItems?.OfType<DeletableUI<Result>>(), QuestionsTable.SelectedItems?.OfType<DeletableUI<Question>>());

        public FactorsView(IReadOnlyFactorsViewState viewState, FactorsController controller)
        {
            this.controller = controller;
            this.dataSource = viewState;
            InitializeComponent();
            ForceUpdateView();

            InputGrid.InputBindings.Add(new KeyBinding(new DelegateCommand(DeleteRows_Press), Key.Delete, ModifierKeys.None));
        }

        private void DeleteRows_Press()
        {
            controller.SoftDelete(ResultsTable.SelectedItems?.OfType<DeletableUI<Result>>(),
                QuestionsTable.SelectedItems?.OfType<DeletableUI<Question>>(),
                WeightsTable.SelectedItems?.OfType<DeletableUI<Weight>>());
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
