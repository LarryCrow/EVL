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

namespace EVL.Views
{
    /// <summary>
    /// Логика взаимодействия для DataImportView.xaml
    /// </summary>
    public partial class DataImportView : UserControl
    {
        private ApplicationModel _model;
        private ImportController _controller;

        public DataImportView(ApplicationModel model, ImportController importController)
        {
            InitializeComponent();
            _model = model;
            _controller = importController;
            ProjectList.ItemsSource = _model.Projects;
            ProjectList.DisplayMemberPath = "Name";
            _model.GetAllProjects();
            QuestionsTable.ItemsSource = _model.questions;
            TypeComboBox.ItemsSource = _model.GetAllQuestionType();
            TypeComboBox.DisplayMemberPath = "Name";
            ViewComboBox.ItemsSource = _model.GetAllQuestionView();
            ViewComboBox.DisplayMemberPath = "Name";
            PurposeComboBox.ItemsSource = _model.GetAllQuestionPurpose();
            PurposeComboBox.DisplayMemberPath = "Name";
        }

        private void ChooseFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            //myDialog.Filter = "CSV Files (*.csv)|*.csv)";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                FilePathInput.Text = myDialog.FileName;
            }
            
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DisplayBtn_Click(object sender, RoutedEventArgs e)
        {
            _controller.ParseFile(FilePathInput.Text, ",", 1, false, 1);
        }
    }
}
