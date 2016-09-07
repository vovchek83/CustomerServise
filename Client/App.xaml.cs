using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly Bootstrapper _bootstrapper;


        public App()
        {


            _bootstrapper = new Bootstrapper();

        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            MessageBox.Show("Please check logs and contact your System Adminstrator.", "Error");
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            _bootstrapper.Initialize();

            base.OnStartup(e);
        }
    }
}

