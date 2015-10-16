using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AdventurePlanner.UI.SourceBookEditor.DataAccess;
using AdventurePlanner.UI.SourceBookEditor.SourceBooks;
using Microsoft.Practices.Unity;

namespace AdventurePlanner.UI.SourceBookEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<SourceBookService>(new ContainerControlledLifetimeManager());

            var mainWindow = new MainWindow();
            mainWindow.DataContext = container.Resolve<MainWindowViewModel>();
            mainWindow.Show();
        }
    }
}
