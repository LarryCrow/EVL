using EVL.Controllers;
using EVL.Model;
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

                var viewState = ViewState.RetrieveDataFrom(model);

                var projectC = new ProjectController(viewState, model);
                var importC = new ImportController(viewState, model);
                var databaseC = new DataBaseController(viewState, model);

                var view = new MainWindow(viewState, importC, projectC, databaseC);

                view.ShowDialog();
            }
        }
    }
}
