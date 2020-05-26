using Chance.DesktopCustomControl.CustomView.MsgDlg;
using DesktopUniversalFrame.Common;
using DesktopUniversalFrame.Entity;
using DesktopUniversalFrame.Model.MedicalModel;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace DesktopUniversalFrame.ViewModel.MedicalViewModel
{
    public class MedicalReportViewModel : WindowCommandBaseModel
    {
        #region Command

        #endregion

        #region Data

        private ObservableCollection<Patient> _patientsInformation;
        /// <summary>
        /// 病人信息集合
        /// </summary>
        public ObservableCollection<Patient> PatientsInformation
        {
            get => _patientsInformation;
            set => SetProperty(ref _patientsInformation, value);
        }

        #endregion

        public MedicalReportViewModel()
        {
            LoadedWindowCommand = new DelegateCommand<Window>(Loaded);
        }

        private void Loaded(Window win)
        {
            Storyboard EndStoryboard = win.Resources["CloseStoryboard"] as Storyboard;
            EndStoryboard.Completed += (sender, e) =>
            {
                win.Close();
            };

            GetPatientData();
        }


        private void GetPatientData()
        {
            PatientsInformation = new ObservableCollection<Patient>();
            for (int i = 1; i < 5; i++)
            {
                var patient = ORMHelper.QueryData<Patient>($"{i}");
                PatientsInformation.Add(ORMHelper.QueryData<Patient>($"{i}"));
            }           
        }

        /// <summary>
        /// 异步委托开启线程
        /// </summary>
        /// <param name="action"></param>
        public static void Action(Action action)
        {
            App.Current.Dispatcher.BeginInvoke(action = () => { });
        }

        private List<Point3D> GetBuckyBallPoints()
        {
            List<Point3D> ltPoints = new List<Point3D>();
            ltPoints.Add(new Point3D(.850651, 0, 2.327438));
            ltPoints.Add(new Point3D(.262866, .809017, 2.327438));
            ltPoints.Add(new Point3D(-.688191, .5, 2.327438));
            ltPoints.Add(new Point3D(-.688191, -.5, 2.327438));
            ltPoints.Add(new Point3D(.262866, -.809017, 2.327438));

            ltPoints.Add(new Point3D(1.701301, 0, 1.801708));
            ltPoints.Add(new Point3D(.52573, 1.618035, 1.801708));
            ltPoints.Add(new Point3D(.52573, -1.618035, 1.801708));
            ltPoints.Add(new Point3D(-1.376383, -.999999, 1.801708));
            ltPoints.Add(new Point3D(-1.376383, .999999, 1.801708));

            ltPoints.Add(new Point3D(1.964166, .809017, 1.275977));
            ltPoints.Add(new Point3D(1.376381, 1.618035, 1.275977));
            ltPoints.Add(new Point3D(-.162461, 2.118035, 1.275977));
            ltPoints.Add(new Point3D(-1.113517, 1.809017, 1.275977));
            ltPoints.Add(new Point3D(-2.064574, .5, 1.275977));
            ltPoints.Add(new Point3D(-2.064574, -.5, 1.275977));
            ltPoints.Add(new Point3D(-1.113517, -1.809017, 1.275977));
            ltPoints.Add(new Point3D(-.162461, -2.118035, 1.275977));
            ltPoints.Add(new Point3D(1.376381, -1.618035, 1.275977));
            ltPoints.Add(new Point3D(1.964166, -.809017, 1.275977));

            ltPoints.Add(new Point3D(2.389492, .5, .425326));
            ltPoints.Add(new Point3D(1.213921, 2.118035, .425326));
            ltPoints.Add(new Point3D(.262865, 2.427051, .425326));
            ltPoints.Add(new Point3D(-1.639248, 1.809017, .425326));
            ltPoints.Add(new Point3D(-2.227033, .999999, .425326));
            ltPoints.Add(new Point3D(-2.227033, -.999999, .425326));
            ltPoints.Add(new Point3D(-1.639248, -1.809017, .425326));
            ltPoints.Add(new Point3D(.262865, -2.427051, .425326));
            ltPoints.Add(new Point3D(1.213921, -2.118035, .425326));
            ltPoints.Add(new Point3D(2.389492, -.5, .425326));

            ltPoints.Add(new Point3D(2.227033, .999999, -.425326));
            ltPoints.Add(new Point3D(1.639248, 1.809017, -.425326));
            ltPoints.Add(new Point3D(-.262865, 2.427051, -.425326));
            ltPoints.Add(new Point3D(-1.213921, 2.118035, -.425326));
            ltPoints.Add(new Point3D(-2.389492, .5, -.425326));
            ltPoints.Add(new Point3D(-2.389492, -.5, -.425326));
            ltPoints.Add(new Point3D(-1.213921, -2.118035, -.425326));
            ltPoints.Add(new Point3D(-.262865, -2.427051, -.425326));
            ltPoints.Add(new Point3D(1.639248, -1.809017, -.425326));
            ltPoints.Add(new Point3D(2.227033, -.999999, -.425326));

            ltPoints.Add(new Point3D(2.064574, .5, -1.275977));
            ltPoints.Add(new Point3D(1.113517, 1.809017, -1.275977));
            ltPoints.Add(new Point3D(.162461, 2.118035, -1.275977));
            ltPoints.Add(new Point3D(-1.376381, 1.618035, -1.275977));
            ltPoints.Add(new Point3D(-1.964166, .809017, -1.275977));
            ltPoints.Add(new Point3D(-1.964166, -.809017, -1.275977));
            ltPoints.Add(new Point3D(-1.376381, -1.618035, -1.275977));
            ltPoints.Add(new Point3D(.162461, -2.118035, -1.275977));
            ltPoints.Add(new Point3D(1.113517, -1.809017, -1.275977));
            ltPoints.Add(new Point3D(2.064574, -.5, -1.275977));

            ltPoints.Add(new Point3D(1.376383, .999999, -1.801708));
            ltPoints.Add(new Point3D(-.52573, 1.618035, -1.801708));
            ltPoints.Add(new Point3D(-1.701301, 0, -1.801708));
            ltPoints.Add(new Point3D(-.52573, -1.618035, -1.801708));
            ltPoints.Add(new Point3D(1.376383, -.999999, -1.801708));

            ltPoints.Add(new Point3D(.688191, .5, -2.327438));
            ltPoints.Add(new Point3D(-.262866, .809017, -2.327438));
            ltPoints.Add(new Point3D(-.850651, 0, -2.327438));
            ltPoints.Add(new Point3D(-.262866, -.809017, -2.327438));
            ltPoints.Add(new Point3D(.688191, -.5, -2.327438));

            return ltPoints;
        }
    }
}
