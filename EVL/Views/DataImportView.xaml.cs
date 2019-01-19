using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
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
            else
            {
                int? projectID = ((Project)ProjectList.SelectedValue)?.Id;
                bool weights = viewState.Questions
                    .Where(q => q.QuestionPurposeName == QuestionPurposeNames.ClientRating 
                                || q.QuestionPurposeName == QuestionPurposeNames.Metric)
                    .All(q => q.Weight.HasValue);

                if (!weights)
                {
                    MessageBox.Show($"Установите веса для объектов типа \"{QuestionPurposeNames.ClientRating}\" и \"{QuestionPurposeNames.Metric}\"");
                }
                else if (projectID.HasValue)
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
                else
                {
                    MessageBox.Show("Выберите проект из списка.");
                }
            }
        }

        private void DisplayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilePathInput.Text))
            {
                MessageBox.Show("Выберите файл для импорта");
                return;
            }

            string separator = ChooseSeparator();
            if (separator == null)
            {
                MessageBox.Show("Выберите разделитель для отображения файла.");
                return;
            }

            controller.ParseFile(FilePathInput.Text, separator);
        }

        private string ChooseSeparator()
        {
            if (TabRB.IsChecked == true)
            {
                return "    ";
            }
            else if (SpaceRB.IsChecked == true)
            {
                return " ";
            }
            else if (PointRB.IsChecked == true)
            {
                return ".";
            }
            else if (SemicolonRB.IsChecked == true)
            {
                return ";";
            }
            else if (CommaRB.IsChecked == true)
            {
                return ",";
            }
            else if (OtherRB.IsChecked == true)
            {
                return OtherSeparatorInput.Text.ToString();
            }
            return null;
        }
    }
}
