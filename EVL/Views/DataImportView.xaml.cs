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
        private readonly IReadOnlyViewState viewState;
        private readonly ImportController controller;

        public DataImportView(IReadOnlyViewState viewState, ImportController importController)
        {
            InitializeComponent();
            this.viewState = viewState;
            this.controller = importController;

            QuestionsTable.ItemsSource = viewState.Questions;
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
            try
            {
                controller.ImportData(viewState.Questions);
                MessageBox.Show("Данные импортированы");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
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

            return TabRB.IsChecked == true ? "\t"
                : SpaceRB.IsChecked == true ? " "
                : PointRB.IsChecked == true ? "."
                : SemicolonRB.IsChecked == true ? ";"
                : CommaRB.IsChecked == true ? ","
                : OtherRB.IsChecked == true && !string.IsNullOrEmpty(os) ? os
                : null;
        }
    }
}
