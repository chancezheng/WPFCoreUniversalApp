using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DesktopUniversalFrame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //App.Current.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            App.Current.StartupUri = new Uri("Views/MedicalView/MedicalReportWindow.xaml", UriKind.Relative);
        }
    }
}
