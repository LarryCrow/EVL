using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EVL.Controllers;
using Model;
using EVL.Model;

namespace EVL.Views
{
    /// <summary>
    /// Логика взаимодействия для DataImportView.xaml
    /// </summary>
    public partial class DataImportView : UserControl
    {
        private IReadOnlyViewState viewState;
        private ImportController controller;

        public DataImportView(IReadOnlyViewState viewState, ImportController importController)
        {
            InitializeComponent();
            this.viewState = viewState;
            this.controller = importController;

            ProjectList.ItemsSource = viewState.Projects;
            QuestionsTable.ItemsSource = viewState.Questions;
            PurposeComboBox.ItemsSource = viewState.QuestionPurposeNames;
        }

        private void ChooseFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog
            {
                //myDialog.Filter = "CSV Files (*.csv)|*.csv)";
                CheckFileExists = true,
                Multiselect = true
            };

            if (myDialog.ShowDialog() == true)
            {
                FilePathInput.Text = myDialog.FileName;
            }

        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            bool segment = viewState.Questions.Any(q => q.QuestionPurposeName == QuestionPurposeNames.Segment);
            bool client = viewState.Questions.Any(q => q.QuestionPurposeName == QuestionPurposeNames.ClientRating);

            int? projectID = ((Project)ProjectList.SelectedValue)?.Id;
            bool weights = viewState.Questions
                .Where(q => q.QuestionPurposeName == QuestionPurposeNames.ClientRating
                          || q.QuestionPurposeName == QuestionPurposeNames.Metric)
                .All(q => q.Weight.HasValue);

            if (segment != true && client != true)
            {
                MessageBox.Show("Выберите поля для сегментирования и формирования клиентской базы." +
                    $"Необходимо присвоить значение {QuestionPurposeNames.Segment} и {QuestionPurposeNames.Characteristic}.");
            }
            else if (segment == true && client != true)
            {
                MessageBox.Show($"Выберите поля для формирования клиентской базы. Необходимо присвоить значение \"{QuestionPurposeNames.Characteristic}\".");
            }
            else if (segment != true && client == true)
            {
                MessageBox.Show($"Выберите поля для сегментирования. Необходимо присвоить значение \"{QuestionPurposeNames.Segment}\".");
            }
            else if (!weights)
            {
                MessageBox.Show($"Установите веса для объектов типа \"{QuestionPurposeNames.ClientRating}\" и \"{QuestionPurposeNames.Metric}\"");
            }
            else if (!projectID.HasValue)
            {
                MessageBox.Show("Выберите проект из списка.");
            }
            else
            {
                try
                {
                    controller.ImportData(projectID.Value, viewState.Questions);
                    MessageBox.Show("Данные импортированы");
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void DisplayBtn_Click(object sender, RoutedEventArgs e)
        {
            string separator = ChooseSeparator();

            if (string.IsNullOrWhiteSpace(FilePathInput.Text))
            {
                MessageBox.Show("Выберите файл для импорта");
            }
            else if (separator == null)
            {
                MessageBox.Show("Выберите разделитель для отображения файла.");
            }
            else
            {
                controller.ParseFile(FilePathInput.Text, separator);
            }
        }

        private string ChooseSeparator()
        {
            var os = OtherSeparatorInput.Text;

            return TabRB.IsChecked == true ? "    "
                : SpaceRB.IsChecked == true ? " "
                : PointRB.IsChecked == true ? "."
                : SemicolonRB.IsChecked == true ? ";"
                : CommaRB.IsChecked == true ? ","
                : OtherRB.IsChecked == true && !string.IsNullOrEmpty(os) ? os
                : null;
        }
    }
}
