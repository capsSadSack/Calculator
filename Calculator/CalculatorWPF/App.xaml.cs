using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CalculatorWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Bootstrapper.Start();

            var window = Bootstrapper.RootScope.Resolve<MainView>();

            window.DataContext = Bootstrapper.RootVisual;

            window.Closed += (s, a) =>
            {
                Bootstrapper.Stop();
            };


            window.Show();

        }
    }
}
