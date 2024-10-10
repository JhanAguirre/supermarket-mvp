using Supermarker_mvp.Models;
using Supermarket_mvp._Repositories;
using Supermarket_mvp.Views;
using Supermarket_mvp.Properties;
using Supermarker_mvp.Presenters;
using System.Configuration;
namespace Supermarket_mvp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            string sqlConnectionString = Settings.Default.Sqlconnection;
            IPayModeRepository repository = new PayModeRepository(sqlConnectionString);
            PayModeView view = new PayModeView();
            new PayModePresenter(view, repository);
            Application.Run((Form)view);
        }
    }
}           