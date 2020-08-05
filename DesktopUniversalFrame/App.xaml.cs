using AutoUpdatedApp;
using DesktopUniversalFrame.AutoUpdate;
using DesktopUniversalFrame.ViewModel.MedicalViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;

namespace DesktopUniversalFrame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ////启动时检查更新
            //UpdateProcess updateProcess = new UpdateProcess();
            //updateProcess.CheckAppVersion();  //检查版本是否最新

            //App.Current.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);

            App.Current.StartupUri = new Uri("Views/MedicalView/MedicalReportWindow.xaml", UriKind.Relative);

            //App.Current.StartupUri = new Uri("Graphics/GraphicsCutting/3D/DemoWindow.xaml", UriKind.Relative);
            //App.Current.StartupUri = new Uri("Graphics/GraphicsCuttingStitching/GraphicsCuttingStitching.xaml", UriKind.Relative);

            //App.Current.StartupUri = new Uri("Charts/ChartWindow.xaml", UriKind.Relative);
        }   
    }
}
