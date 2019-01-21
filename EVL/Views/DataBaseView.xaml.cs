using System;
using System.Collections.Generic;
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
using EVL.Model;
using EVL.Controllers;

namespace EVL.Views
{
    /// <summary>
    /// Логика взаимодействия для DataBaseView.xaml
    /// </summary>
    public partial class DataBaseView : UserControl
    {
        private readonly ReadOnlyDatabaseViewState viewState;

        public DataBaseView(ReadOnlyDatabaseViewState viewState)
        {
            InitializeComponent();

            this.viewState = viewState;
            ClientsTable.ItemsSource = viewState.Companies;
        }

        private void ClientsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = e.AddedItems.OfType<CompanyUI>().Single();
            QuestionsTable.ItemsSource = viewState.GetBlanket(selected);
        }
    }
}
