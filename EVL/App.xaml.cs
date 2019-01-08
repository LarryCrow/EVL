using EVL.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Model;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace EVL
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application, IDesignTimeDbContextFactory<DataBaseContext>
    {
        // required for migrations (maybe refactor)
        public DataBaseContext CreateDbContext(string[] args)
        {
            var connString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            var options = new DbContextOptionsBuilder().UseSqlite(connString).Options;
            return new DataBaseContext(options);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            using(var model = CreateDbContext(null))
            {
                model.Database.Migrate();

                var controller = new ProjectC(new ApplicationModel(model));
                var view = new MainWindow(controller);

                view.ShowDialog();
            }
        }
    }
}
