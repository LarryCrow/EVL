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
        private readonly DbContextOptions<DataBaseContext> options;

        public App()
        {
            var connString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            options = new DbContextOptionsBuilder<DataBaseContext>().UseSqlite(connString).Options;
        }

        // required for migrations (maybe refactor)
        public DataBaseContext CreateDbContext(string[] args) => new DataBaseContext(options);
        private DataBaseContext CreateDbContext() => CreateDbContext(null);


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            using (var model = CreateDbContext()) { model.Database.Migrate(); }

            var viewState = ViewState.RetrieveDataFrom(CreateDbContext());

            var projectC = new ProjectController(viewState, CreateDbContext);
            var importC = new ImportController(viewState, CreateDbContext);

            var view = new MainWindow(viewState, importC, projectC);

            view.ShowDialog();
        }
    }
}
