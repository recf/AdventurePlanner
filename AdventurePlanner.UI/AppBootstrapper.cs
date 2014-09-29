using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AdventurePlanner.UI.ViewModels;
using Caliburn.Micro;

namespace AdventurePlanner.UI
{
    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e) 
        {
            DisplayRootViewFor<CharacterPlanViewModel>();
        }
    }
}
