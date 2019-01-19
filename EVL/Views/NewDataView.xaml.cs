using EVL.Controllers;
using EVL.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EVL.Views
{
    /// <summary>
    /// Interaction logic for NewDataView.xaml
    /// </summary>
    public partial class NewDataView : UserControl
    {
        private readonly NewDataController controller;
        private readonly IReadOnlyViewState viewState;

        public NewDataView(IReadOnlyViewState viewState, NewDataController controller)
        {
            InitializeComponent();

            this.controller = controller;
            this.viewState = viewState;
            MetricsTable.ItemsSource = this.viewState.MetricQA;
            CharacteristicTable.ItemsSource = this.viewState.CharacteristicQA;
            RatingsTable.ItemsSource = this.viewState.ClientRatingQA;

            this.controller.FillTable();
        }
    }
}
