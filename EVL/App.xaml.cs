using EVL.Controllers;
using EVL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Model;
using System.Configuration;
using System.Windows;

namespace EVL
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application, IDesignTimeDbContextFactory<EvlContext>
    {
        private readonly DbContextOptions<EvlContext> options;

        public App()
        {
            var connString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            options = new DbContextOptionsBuilder<EvlContext>().UseSqlite(connString).Options;
        }

        // required for migrations (maybe refactor)
        public EvlContext CreateDbContext(string[] args) => new EvlContext(options);
        private EvlContext CreateDbContext() => CreateDbContext(null);


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            using (var model = CreateDbContext()) { model.Database.Migrate(); }

            var viewState = ViewState.RetrieveDataFrom(CreateDbContext());

            var view = new MainWindow(viewState, new MainController(viewState, CreateDbContext));

            view.ShowDialog();
        }
    }
}
