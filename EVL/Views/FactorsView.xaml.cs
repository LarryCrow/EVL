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
    /// Логика взаимодействия для FactorsView.xaml
    /// </summary>
    public partial class FactorsView : UserControl
    {
        private readonly FactorsController controller;
        private readonly IReadOnlyViewState viewState;

        public FactorsView(IReadOnlyViewState viewState, FactorsController controller)
        {
            InitializeComponent();

            this.controller = controller;
            this.viewState = viewState;
        }
    }
}
