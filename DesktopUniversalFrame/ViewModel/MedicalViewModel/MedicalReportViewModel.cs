using Chance.DesktopCustomControl.CustomComponent;
using Chance.DesktopCustomControl.CustomView.MsgDlg;
using DesktopUniversalFrame.Common;
using DesktopUniversalFrame.Entity;
using DesktopUniversalFrame.Entity.CustomValidationRules;
using DesktopUniversalFrame.Model.MedicalModel;
using DesktopUniversalFrame.Views.InterfaceView;
using DesktopUniversalFrame.Views.MedicalView;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace DesktopUniversalFrame.ViewModel.MedicalViewModel
{
    public class MedicalReportViewModel : WindowCommandBaseModel, IWindowService
    {

        #region Command

        private DelegateCommand<RadioButton> _functionTabSwitch;
        private DelegateCommand<DataGrid> _loadingRowCommand;
        private DelegateCommand<DataGrid> _selectionChangedCommand;
        private DelegateCommand<CheckBox> _selectedAllCommand;
        private DelegateCommand<CheckBox> _selectedThisCommand;
        private DelegateCommand<MenuItem> _contextMenuItemSelectedCommand;


        /// <summary>
        /// 功能切换
        /// </summary>
        public DelegateCommand<RadioButton> FunctionTabSwitch
        {
            get => _functionTabSwitch;
            set => SetProperty(ref _functionTabSwitch, value);
        }

        /// <summary>
        /// 读出DataGrid信息
        /// </summary>
        public DelegateCommand<DataGrid> LoadingRowCommand
        {
            get { return _loadingRowCommand; }
            set { _loadingRowCommand = value; }
        }

        /// <summary>
        /// 行切换
        /// </summary>
        public DelegateCommand<DataGrid> SelectionChangedCommand
        {
            get { return _selectionChangedCommand; }
            set { _selectionChangedCommand = value; }
        }

        /// <summary>
        /// 选择全部
        /// </summary>
        public DelegateCommand<CheckBox> SelectedAllCommand
        {
            get { return _selectedAllCommand; }
            set { _selectedAllCommand = value; }
        }

        /// <summary>
        /// 单项选择
        /// </summary>
        public DelegateCommand<CheckBox> SelectedThisCommand
        {
            get { return _selectedThisCommand; }
            set { _selectedThisCommand = value; }
        }

        /// <summary>
        /// 菜单选择
        /// </summary>
        public DelegateCommand<MenuItem> ContextMenuItemSelectedCommand
        {
            get { return _contextMenuItemSelectedCommand; }
            set { _contextMenuItemSelectedCommand = value; }
        }


        #endregion

        #region Data

        private ObservableCollection<PatientExtention> _patientsInformation;
        private ObservableCollection<ReportFunctionInfo> _reportFunctionInfo = new ObservableCollection<ReportFunctionInfo>
        {
            new ReportFunctionInfo{
                Function = "挂号",FunctionItems =
                new List<FunctionItems>{
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "挂号"},
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "导入"},
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "导出"},
                },
            },
            new ReportFunctionInfo{
                Function = "诊断",FunctionItems =
                new List<FunctionItems>{
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "诊断"},
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "导入"},
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "导出"},
                },
            },
            new ReportFunctionInfo{
                Function = "复核",FunctionItems =
                new List<FunctionItems>{
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "复核"},
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "导入"},
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "导出"},
                },
            },
            new ReportFunctionInfo{
                Function = "报告",FunctionItems =
                new List<FunctionItems>{
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "报告"},
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "导入"},
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "导出"},
                },
            },
           new ReportFunctionInfo{
                Function = "更多",FunctionItems =
                new List<FunctionItems>{
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "更多"},
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "导入"},
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "导出"},
                },
            },
        };
        private bool? _isSelectedAllItems;
        private int _selectedCount;


        /// <summary>
        /// 病人信息集合
        /// </summary>
        public ObservableCollection<PatientExtention> PatientsInformation
        {
            get => _patientsInformation;
            set => SetProperty(ref _patientsInformation, value);
        }

        /// <summary>
        /// 功能列表
        /// </summary>
        public ObservableCollection<ReportFunctionInfo> ReportFunctionInfo
        {
            get => _reportFunctionInfo;
            set => SetProperty(ref _reportFunctionInfo, value);
        }

        /// <summary>
        /// 选中行数
        /// </summary>
        public int SelectedCount
        {
            get => _selectedCount;
            set => SetProperty(ref _selectedCount, value);
        }

        /// <summary>
        /// 是否选择全部
        /// </summary>
        public bool? IsSelectedAllItems
        {
            get => _isSelectedAllItems;
            set => SetProperty(ref _isSelectedAllItems, value);
        }

        #endregion


        public MedicalReportViewModel()
        {
            LoadedWindowCommand = new DelegateCommand<Window>(Loaded);
            FunctionTabSwitch = new DelegateCommand<RadioButton>(SelectTabItem);
            LoadingRowCommand = new DelegateCommand<DataGrid>(LoadingRow);
            SelectionChangedCommand = new DelegateCommand<DataGrid>(SelectionChanged);
            SelectedAllCommand = new DelegateCommand<CheckBox>(SelectedAll);
            SelectedThisCommand = new DelegateCommand<CheckBox>(SelectedThisItem);
            ContextMenuItemSelectedCommand = new DelegateCommand<MenuItem>(ContextMenuItemSelected);

            PatientsInformation = GetPatientData();
        }

        private void Loaded(Window win)
        {
            #region Loaded动画

            Storyboard EndStoryboard = win.Resources["CloseStoryboard"] as Storyboard;
            EndStoryboard.Completed += (sender, e) =>
            {
                win.Close();
            };

            #endregion

            ContextMenu contextMenu = win.FindName("contextMenu") as ContextMenu;
            foreach (var item in contextMenu.Items)
            {
                if (item is MenuItem)
                {
                    (item as MenuItem).Click += delegate { ContextMenuItemSelected(item as MenuItem); };
                }
            }
        }

        //获取病人信息数据
        private ObservableCollection<PatientExtention> GetPatientData()
        {
            ObservableCollection<PatientExtention> patientsInformation = new ObservableCollection<PatientExtention>();
            for (int i = 1; i < 10; i++)
            {
                var p = ORMHelper.QueryData<PatientExtention>($"{i}");
                if (p != null)
                    patientsInformation.Add(p);
            }

            return patientsInformation;
        }

        //功能选择器
        private void SelectTabItem(RadioButton rb)
        {
            switch (rb.Tag.ToString())
            {
                case "挂号":
                    {
                        RegistrationViewModel registrationViewModel = new RegistrationViewModel();
                        registrationViewModel.PatientRegister = new PatientExtention();
                        registrationViewModel.IsUIVisible = true;
                        ShowWindow(registrationViewModel, rb);
                    }
                    break;
                case "导入":
                    {
                        var ss = PatientsInformation.Count(item => item.IsSelected);
                        MessageBox.Show(ss.ToString());
                    }
                    break;
                case "导出":
                    {

                    }
                    break;
                default:
                    break;
            }
        }

        #region DataGrid

        private void LoadingRow(DataGrid dg)
        {
            DataGridCheckBoxColumn dataGridCheckBoxColumn = new DataGridCheckBoxColumn();
            //e.Row.MouseLeftButtonUp += (s, e) =>
            //{
            //    (sender as DataGrid).SelectedIndex = (s as DataGridRow).GetIndex();
            //    (s as DataGridRow).Focus();
            //    e.Handled = true;
            //};
        }

        //单项选择
        private void SelectedThisItem(CheckBox chk)
        {
            SelectedCount = PatientsInformation.Count(item => item.IsSelected);
        }

        //选择全部
        private void SelectedAll(CheckBox chk)
        {
            if (chk.IsChecked == false)
            {
                foreach (var item in PatientsInformation)
                {
                    item.IsSelected = false;
                }
            }
            else if (chk.IsChecked == true)
            {
                foreach (var item in PatientsInformation)
                {
                    item.IsSelected = true;
                }
            }

            SelectedCount = PatientsInformation.Count(item => item.IsSelected);
        }

        private static int selectedIndex = -1;
        private void SelectionChanged(DataGrid dg)
        {
            selectedIndex = dg.SelectedIndex;
        }

        private void ContextMenuItemSelected(MenuItem item)
        {
            switch (item.Name)
            {
                case "watch":
                    {
                        if (selectedIndex < 0)
                        {
                            MessageDialog.Show("未选中行");
                            return;
                        }
                        RegistrationViewModel registrationViewModel = new RegistrationViewModel();
                        registrationViewModel.PatientRegister = PatientsInformation[selectedIndex];
                        registrationViewModel.IsUIVisible = false;
                        ShowWindow(registrationViewModel, item);
                    }
                    break;
                case "register":
                    {
                        RegistrationViewModel registrationViewModel = new RegistrationViewModel();
                        registrationViewModel.PatientRegister = new PatientExtention();
                        registrationViewModel.IsUIVisible = true;
                        ShowWindow(registrationViewModel, item);
                    }
                    break;
                case "editor":
                    {
                        if (selectedIndex < 0)
                        {
                            MessageDialog.Show("未选中行");
                            return;
                        }
                        RegistrationViewModel registrationViewModel = new RegistrationViewModel();
                        registrationViewModel.PatientRegister = PatientsInformation[selectedIndex];
                        registrationViewModel.IsUIVisible = true;
                        ShowWindow(registrationViewModel, item);
                        
                    }
                    break;
                case "update":
                    {
                        Refresh();
                        selectedIndex = -1;
                    }
                    break;
                case "delete":
                    {
                        ORMHelper.Delete<PatientExtention>(PatientsInformation[selectedIndex]);
                        Refresh();
                    }
                    break;
                default:
                    break;
            }           
        }

        #endregion

        #region 挂号功能

        #endregion

        /// <summary>
        /// 刷新
        /// </summary>
        private async void Refresh()
        {          
            ObservableCollection<PatientExtention> tsNew = new ObservableCollection<PatientExtention>();            
            await Task.Factory.StartNew(() =>
            {
                tsNew = GetPatientData();
                PatientsInformation = null;
                Thread.Sleep(200);
                PatientsInformation = tsNew;
            });

            //CommandManager.InvalidateRequerySuggested();
            //Task.Factory.StartNew(() => Console.WriteLine(123), new CancellationTokenSource().Token, 
            //    TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()).Wait();
        }

        /// <summary>
        /// 开启窗口
        /// </summary>
        public void ShowWindow(object ViewModel, FrameworkElement frameworkElement)
        {
            Registration registration = new Registration();
            registration.DataContext = ViewModel;
            registration.Owner = Window.GetWindow(frameworkElement) as MedicalReportWindow;
            registration.Show();
            //registration.ShowDialog();
        }

        /// <summary>
        /// 异步更新UI
        /// </summary>
        /// <param name="action"></param>
        public static void Action(Action action)
        {
            App.Current.Dispatcher.BeginInvoke(action = () => { });
        }
    }
}
