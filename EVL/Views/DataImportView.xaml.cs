﻿using Microsoft.Win32;
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
            TypeComboBox.ItemsSource = viewState.QuestionTypeNames;
            ViewComboBox.ItemsSource = viewState.QuestionViewNames;
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

            bool everyoneHasType = viewState.Questions.All(q => !string.IsNullOrWhiteSpace(q.QuestionTypeName));

            if (segment != true && client != true)
            {
                MessageBox.Show("Выберите поля для сегментирования и формирования клиентской базы." +
                    "Необходимо присвоить значение Сегмент и Название клиента.");
            }
            else if (segment == true && client != true)
            {
                MessageBox.Show("Выберите поля для формирования клиентской базы. Необходимо присвоить значение Название клиента.");
            }
            else if (segment != true && client == true)
            {
                MessageBox.Show("Выберите поля для сегментирования. Необходимо присвоить значение Сегмент.");
            }
            else
            {
                try
                {
                    controller.ImportData(viewState.Questions, 1);
                    MessageBox.Show("Данные импортированы");
                }
                catch(InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void DisplayBtn_Click(object sender, RoutedEventArgs e)
        {
            controller.ParseFile(FilePathInput.Text, ",", 1);
        }
    }
}
