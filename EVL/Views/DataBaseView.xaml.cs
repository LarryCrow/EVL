using System.Linq;
using System.Windows.Controls;
using EVL.Model;

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
