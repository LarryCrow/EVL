using EVL.Controllers;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Configuration;
using System.Windows;

namespace EVL
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var connString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            var options = new DbContextOptionsBuilder().UseSqlite(connString).Options;

            using(var model = new DataBaseContext(options))
            {
                model.Database.EnsureCreated();

                var controller = new ProjectC(model);
                var view = new MainWindow(controller);

                view.ShowDialog();
            }
        }
    }
}
