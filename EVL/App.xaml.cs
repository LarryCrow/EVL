using EVL.Controllers;
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

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            using (var model = new EvlContext(options)) { model.Database.Migrate(); }

            new MainWindow(new MainController(() => new EvlContext(options))).ShowDialog();
        }

        // required for migrations (maybe refactor)
        EvlContext IDesignTimeDbContextFactory<EvlContext>.CreateDbContext(string[] args) => new EvlContext(options);

    }
}
