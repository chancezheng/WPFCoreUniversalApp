using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoUpdatedApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window, INotifyPropertyChanged
    {
        private double _currentValue;      
        public double CurrentValue
        {
            get { return _currentValue; }
            set { _currentValue = value; OnPropertyChanged(); }
        }

        private string _processInfo;
        public string ProcessInfo
        {
            get { return _processInfo; }
            set { _processInfo = value; OnPropertyChanged(); }
        }



        public UpdateWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DownLoadFiles("https://www.baidu.com");

            //Task.Run(() => Test());
            //增加软件自动更新和手动更新功能
        }

        private void Test()
        {
            int i = 1;
            do
            {
                i++;
                CurrentValue = i;
                ProcessInfo = $"已下载({CurrentValue}%)";
                Thread.Sleep(200);
            } while (i < 100); 
        }

        private void DownLoadFiles(string url)
        {
            var client = new WebClient();
            client.DownloadDataAsync(new Uri(url));
            client.DownloadProgressChanged += (obj, e) =>
            {
                UpdateProcess(e.BytesReceived, e.TotalBytesToReceive);
            };

            client.DownloadDataCompleted += (obj, e) =>
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;
                path = Directory.GetParent(path).Parent.Parent.FullName;
                //Process.Start(path + "\\DesktopUniversalFrame.exe");

                //Thread thread = new Thread(() =>
                //{
                //    Thread.Sleep(1000);
                //    Dispatcher.Invoke(() => this.Close());
                //});
                //thread.Start();              
            };          
        }

        private void UpdateProcess(long current, long total)
        {
            CurrentValue = (int)(current * 100F / total);
            ProcessInfo = $"已下载({CurrentValue}%)";
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
