using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EVL.Controllers;
using EVL.Model;

namespace EVL.Views
{
    /// <summary>
    /// Interaction logic for NewDataView.xaml
    /// </summary>
    public partial class NewDataView : UserControl
    {
        private readonly NewDataController controller;
        private readonly IReadOnlyNewDataViewState viewState;

        public NewDataView(IReadOnlyNewDataViewState viewState, NewDataController controller)
        {
            this.controller = controller;
            this.viewState = viewState;
            InitializeComponent();

            QuestionsTable.ItemsSource = this.viewState.QAList;
        }

        private void CalculateLoyalty_Click(object sender, RoutedEventArgs e)
        {
            var result = controller.Calculate().OrderByDescending(p => p.ConditionalProbability).Select(r => $"{r.Result.Name}: {r.ConditionalProbability:0.000}");
            MessageBox.Show($"Results:\n{string.Join(Environment.NewLine, result)}");
        }
    }
}
