using LiveCharts;
using LiveCharts.Geared;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using System.Windows.Shapes;

namespace DesktopUniversalFrame.Charts
{
    /// <summary>
    /// ChartWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChartWindow : Window, INotifyPropertyChanged, IDisposable
    {
        private double _trend;
        private double _count;
        private double _currentLecture;
        private bool _isHot;

        public ChartWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            InitCommand();

            PieLoaded();
        }

        private void InitCommand()
        {
            LineValues = new GearedValues<double>().WithQuality(Quality.Highest);
            PieValues = new GearedValues<double>().WithQuality(Quality.Highest);
            ReadCommand = new DelegateCommand(Read);
            StopCommand = new DelegateCommand(Stop);
            CleaCommand = new DelegateCommand(Clear);
        }

        
        public DelegateCommand ReadCommand { get; set; }
        public DelegateCommand StopCommand { get; set; }
        public DelegateCommand CleaCommand { get; set; }
        public bool IsReading { get; set; }
        public GearedValues<double> LineValues { get; set; }
        public GearedValues<double> PieValues { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }

        public double Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }

        public double CurrentLecture
        {
            get { return _currentLecture; }
            set
            {
                _currentLecture = value;
                OnPropertyChanged("CurrentLecture");
            }
        }

        public bool IsHot
        {
            get { return _isHot; }
            set
            {
                var changed = value != _isHot;
                _isHot = value;
                if (changed) OnPropertyChanged("IsHot");
            }
        }



        private void Stop()
        {
            IsReading = false;
        }

        private void Clear()
        {
            LineValues.Clear();
        }

        private void Read()
        {
            if (IsReading) return;

            //lets keep in memory only the last 20000 records,
            //to keep everything running faster
            const int keepRecords = 20000;
            IsReading = true;

            Action readFromTread = () =>
            {
                while (IsReading)
                {
                    Thread.Sleep(1);
                    var r = new Random();
                    _trend += (r.NextDouble() < 0.5 ? 1 : -1) * r.Next(0, 10) * .001;
                    //when multi threading avoid indexed calls like -> Values[0] 
                    //instead enumerate the collection
                    //ChartValues/GearedValues returns a thread safe copy once you enumerate it.
                    //TIPS: use foreach instead of for
                    //LINQ methods also enumerate the collections
                    var first = LineValues.DefaultIfEmpty(0).FirstOrDefault();
                    if (LineValues.Count > keepRecords - 1) LineValues.Remove(first);
                    if (LineValues.Count < keepRecords) LineValues.Add(_trend);
                    IsHot = _trend > 0;
                    Count = LineValues.Count;
                    CurrentLecture = _trend;
                }
            };

            //2 different tasks adding a value every ms
            //add as many tasks as you want to test this feature
            Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
            //Task.Factory.StartNew(readFromTread);
        }


        private void PieLoaded()
        {
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
      
            //Task.Run(() =>
            //{
            //    double[] counts = new double[5] { 1.2, 3.2, 4.0, 3.5, 6.0 };
            //    foreach (var item in counts)
            //    {
            //        PieValues.Add(item);
            //    }
            //});
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            this.LineValues.Dispose();
        }
    }
}
