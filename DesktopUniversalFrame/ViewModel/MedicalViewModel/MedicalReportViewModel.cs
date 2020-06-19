using DesktopUniversalCustomControl.CustomView.MsgDlg;
using DesktopUniversalFrame.Common.MappingAttribute;
using DesktopUniversalFrame.Entity;
using DesktopUniversalFrame.Entity.FileUtils;
using DesktopUniversalFrame.Model.MedicalModel;
using DesktopUniversalFrame.Views.InterfaceView;
using DesktopUniversalFrame.Views.MedicalView;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace DesktopUniversalFrame.ViewModel.MedicalViewModel
{
    public delegate void MessengerViewModel(List<PatientExtention> patients);

    public class MedicalReportViewModel : WindowCommandBaseModel, IWindowService
    {
        public MessengerViewModel MessengerViewModelDelegate { get; set; }
        private DiagnoseViewModel DiagnoseViewModel { get; set; }

        #region Command

        private DelegateCommand<Hyperlink> _giteeHyperlinkCommand;
        private DelegateCommand<RadioButton> _functionTabSwitch;
        private DelegateCommand<DataGrid> _loadingRowCommand;
        private DelegateCommand<DataGrid> _selectionChangedCommand;
        private DelegateCommand<CheckBox> _selectedAllCommand;
        private DelegateCommand<CheckBox> _selectedThisCommand;
        private DelegateCommand<MenuItem> _contextMenuItemSelectedCommand;


        /// <summary>
        /// Gitee
        /// </summary>
        public DelegateCommand<Hyperlink> GiteeHyperlinkCommand
        {
            get { return _giteeHyperlinkCommand; }
            set { _giteeHyperlinkCommand = value; }
        }

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
                    new FunctionItems { Icon = PackIconKind.Plus, Operation = "挂号", Code = "Registration.register"},
                    new FunctionItems { Icon = PackIconKind.FileExcel, Operation = "Excel模板", Code = "Registration.excel"},
                    new FunctionItems { Icon = PackIconKind.Import, Operation = "导入", Code = "Registration.import"},
                    new FunctionItems { Icon = PackIconKind.Export, Operation = "导出", Code = "Registration.export"},
                },
            },
            new ReportFunctionInfo{
                Function = "诊断",FunctionItems =
                new List<FunctionItems>{
                    new FunctionItems { Icon = PackIconKind.Library, Operation = "本地诊断", Code = "Diagnose.local"},
                    new FunctionItems { Icon = PackIconKind.Cloud, Operation = "云诊断", Code = "Diagnose.cloud"},
                    new FunctionItems { Icon = PackIconKind.EyeCheck, Operation = "自动诊断", Code = "Diagnose.auto"},
                },
            },
            new ReportFunctionInfo{
                Function = "复核",FunctionItems =
                new List<FunctionItems>{
                    new FunctionItems { Icon = PackIconKind.LocalActivity, Operation = "本地复核"},
                    new FunctionItems { Icon = PackIconKind.Cloud, Operation = "云复核"},
                    new FunctionItems { Icon = PackIconKind.HeadCheck, Operation = "自动复核"},
                },
            },
            new ReportFunctionInfo{
                Function = "报告",FunctionItems =
                new List<FunctionItems>{
                    new FunctionItems { Icon = PackIconKind.Printer, Operation = "打印"},
                    new FunctionItems { Icon = PackIconKind.Pdf, Operation = "PDF"},
                    new FunctionItems { Icon = PackIconKind.FileWord, Operation = "Word"},
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
            set 
            {
                if (_patientsInformation == value) return;              
                SetProperty(ref _patientsInformation, value);
                MessengerViewModelDelegate?.Invoke(PatientsInformation.ToList());
            }
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

        #region Fileds

        private static int selectedIndex = -1;
        private static List<PatientExtention> selectedPatients = new List<PatientExtention>();
        private static int indexItem = 1;
        private List<string> execelModelTitle = new List<string>
        {
            "姓名","年龄","性别","电话",
            "样本号","样本类型","条码号","报告类型","状态",
            "送检医院","送检科室","送检医生","门诊号",
            "挂号日期(yyyy/MM/dd)","导出日期(yyyy/MM/dd)","打印日期(yyyy/MM/dd)",
        };

        #endregion


        public MedicalReportViewModel()
        {
            SubscriptionEvent();

            LoadedWindowCommand = new DelegateCommand<Window>(Loaded);
            FunctionTabSwitch = new DelegateCommand<RadioButton>(SelectTabItem);
            LoadingRowCommand = new DelegateCommand<DataGrid>(LoadingRow);
            SelectionChangedCommand = new DelegateCommand<DataGrid>(SelectionChanged);
            SelectedAllCommand = new DelegateCommand<CheckBox>(SelectedAll);
            SelectedThisCommand = new DelegateCommand<CheckBox>(SelectedThisItem);
            //ContextMenuItemSelectedCommand = new DelegateCommand<MenuItem>(ContextMenuItemSelected);

            GiteeHyperlinkCommand = new DelegateCommand<Hyperlink>(GitHyperink);            
        }

        //订阅事件
        private void SubscriptionEvent()
        {
            DiagnoseViewModel = new DiagnoseViewModel();
            MessengerViewModelDelegate += DiagnoseViewModel.MessengerViewModelDelegate;
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

            PatientsInformation = GetPatientData();          
        }

        //获取病人信息数据
        private ObservableCollection<PatientExtention> GetPatientData()
        {
            ObservableCollection<PatientExtention> patientsInformation = new ObservableCollection<PatientExtention>();
            var Items = ORMHelper.QueryData<PatientExtention>();
            if (Items == null)
                return null;

            indexItem = 1;
            foreach (var item in Items)
            {
                item.IndexOfItem = indexItem++;
                patientsInformation.Add(item);
            }
            return patientsInformation;
        }

        //功能选择器
        private void SelectTabItem(RadioButton rb)
        {
            switch (rb.Tag.ToString())
            {
                case "Registration.register":
                    {
                        RegistrationViewModel registrationViewModel = new RegistrationViewModel();
                        registrationViewModel.PatientRegister = new PatientExtention();
                        registrationViewModel.IsUIVisible = true;
                        ShowWindow<Registration>(registrationViewModel, rb);
                    }
                    break;
                case "Registration.excel":
                    {
                        CreateExcelModel();
                    }
                    break;
                case "Registration.import":
                    {
                        ImportExcelData();
                    }
                    break;
                case "Registration.export":
                    {
                        ExportExcelData();
                    }
                    break;
                case "Diagnose.local":
                    {
                        DiagnoseViewModel diagnoseViewModel = new DiagnoseViewModel();
                        ShowWindow<DiagnoseView>(diagnoseViewModel, rb);
                    }
                    break;
                case "Diagnose.cloud":
                    {

                    }
                    break;
                case "Diagnose.auto":
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
            //dg.LoadingRow += (sender, e) =>
            //{
            //    e.Row.MouseLeftButtonUp += (s, e) =>
            //    {
            //        (sender as DataGrid).SelectedIndex = (s as DataGridRow).GetIndex();
            //        selectedIndex = (s as DataGridRow).GetIndex();
            //        (s as DataGridRow).Focus();
            //        e.Handled = true;
            //    };
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
      
        private void SelectionChanged(DataGrid dg)
        {
            selectedIndex = dg.SelectedIndex;
            foreach (PatientExtention item in dg.SelectedItems)
            {
                if (!selectedPatients.Contains(item))
                    selectedPatients.Add(item);
            }
        }

        //菜单选择
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
                        ShowWindow<Registration>(registrationViewModel, item);
                    }
                    break;
                case "register":
                    {
                        RegistrationViewModel registrationViewModel = new RegistrationViewModel();
                        registrationViewModel.PatientRegister = new PatientExtention();
                        registrationViewModel.IsUIVisible = true;
                        ShowWindow<Registration>(registrationViewModel, item);
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
                        ShowWindow<Registration>(registrationViewModel, item);

                    }
                    break;
                case "update":
                    {
                        Refresh();
                    }
                    break;
                case "delete":
                    {
                        for (int i = 0; i < selectedPatients.Count; i++)
                        {
                            ORMHelper.Delete<PatientExtention>(selectedPatients[i]);
                            PatientsInformation.Remove(selectedPatients[i]); //PatientInfomation每次更新都会引起SelectionChanged事件发生,所以这句话一定要在后面
                        }
                        Refresh();
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region 挂号功能

        //创建Excel模板
        private void CreateExcelModel()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel文件|*.xls;*.xlsx;*.csv";
                saveFileDialog.DefaultExt = ".xlsx";
                saveFileDialog.Title = "Excel模板";
                saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Excel模板";
                //saveFileDialog.CheckFileExists = true;
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.FileName = "Excel模板" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒");

                if (saveFileDialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                {
                    FileOutputUtil.OutputDir = Directory.GetParent(saveFileDialog.FileName);
                    //设置许可证，不设置的异常仅在连接调试器时引发，因此您不必在生产/发布环境中配置此异常
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage())
                    {
                        //Add a new worksheet to the empty workbook
                        var workSheet = package.Workbook.Worksheets.Add("病人信息统计表");
                        //Add the headers
                        for (int i = 1; i <= execelModelTitle.Count; i++)
                        {
                            workSheet.Cells[1, i].Value = execelModelTitle[i - 1];                           
                        }

                        //设置表头样式
                        using (var range = workSheet.Cells[1,1,1,execelModelTitle.Count])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BlueViolet);
                            range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        }
                        //设置单元格格式
                        //workSheet.Cells["B2:B5"].Style.Numberformat.Format = "#,##0";
                        //workSheet.Cells["D2"].Style.Numberformat.Format = "#,##0";
                        //workSheet.Cells["N2:N4"].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                        //workSheet.Cells["O2:N"].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                        //workSheet.Cells["P2:N"].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";

                        //Auto Calculaye
                        //workSheet.Calculate();

                        //Autofit columns for all cells
                        workSheet.Cells.AutoFitColumns();
                        //Change the sheet view to show it in page layout mode
                        workSheet.View.PageLayoutView = true;
                        var xlsxFile = FileOutputUtil.GetFileInfo(saveFileDialog.SafeFileName);
                        package.SaveAs(xlsxFile);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
        }

        //导入Excel数据
        private void ImportExcelData()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel文件|*.xls;*.xlsx;*.csv";
                if (openFileDialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
                {
                    FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(fs))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var ss = worksheet.Name;
                        int minColumNum = worksheet.Dimension.Start.Column; //开始列
                        int maxColumNum = worksheet.Dimension.End.Column; //结束列
                        int minRowNum = worksheet.Dimension.Start.Row; //开始行
                        int maxRowNum = worksheet.Dimension.End.Row; //结束行
                     
                        for (int i = 2; i <= maxRowNum; i++)
                        {
                            Type type = typeof(PatientExtention);
                            var instance = Activator.CreateInstance<PatientExtention>();
                            var hh = type.GetProperties().ExceptKey().ExcepteIgnoreProperty().ToList();
                            for (int j = 1; j <= maxColumNum; j++)
                            {
                                var v = worksheet.Cells[i, j].Value;
                                TypeConverterTo(hh[j - 1], instance, v);
                            }
                            //PatientsInformation.Add(instance);
                            bool isInsert = ORMHelper.InsertData(instance);
                        }
                    }
                    Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
        }

        //类型转换
        private void TypeConverterTo(PropertyInfo prop, PatientExtention p,object value)
        {
            if(prop.PropertyType == typeof(string))
            {
                prop.SetValue(p, Convert.ToString(value));
            }
            else if (prop.PropertyType == typeof(int))
            {
                prop.SetValue(p, Convert.ToInt32(value));
            }
            else if (prop.PropertyType == typeof(Gender))
            {
                if(value.ToString() == "男")
                    prop.SetValue(p, Gender.Male);
                else if (value.ToString() == "女")
                    prop.SetValue(p, Gender.Female);
                else
                    prop.SetValue(p, Gender.Unknow);
            }
            else if (prop.PropertyType == typeof(DiagnoseState))
            {
                if (value.ToString() == "未诊断")
                    prop.SetValue(p, DiagnoseState.Undiagnose);
                else if (value.ToString() == "已诊断")
                    prop.SetValue(p, DiagnoseState.Diagnosed);
                else if(value.ToString() == "已复核")
                    prop.SetValue(p, DiagnoseState.Reviewered);
            }
            else if (prop.PropertyType == typeof(DateTime))
            {
                //prop.SetValue(p, Convert.ToDateTime(DateTime.Parse(value.ToString()));
                prop.SetValue(p, DateTime.FromOADate((double)value));
            }
            else
                prop.SetValue(p, (string)value);
        }

        //导出Excel数据
        private void ExportExcelData()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel文件|*.xls;*.xlsx;*.csv";
                saveFileDialog.DefaultExt = ".xlsx";
                saveFileDialog.Title = "Excel模板";
                saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Excel模板";
                //saveFileDialog.CheckFileExists = true;
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.FileName = "Excel模板" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒");

                if (saveFileDialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                {
                    FileOutputUtil.OutputDir = Directory.GetParent(saveFileDialog.FileName);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage())
                    {
                        var workSheet = package.Workbook.Worksheets.Add("病人信息统计表");
                        for (int i = 1; i <= execelModelTitle.Count; i++)
                        {
                            workSheet.Cells[1, i].Value = execelModelTitle[i - 1];
                        }

                        //设置表头样式
                        using (var range = workSheet.Cells[1, 1, 1, execelModelTitle.Count])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BlueViolet);
                            range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        }

                        for (int i = 0; i < PatientsInformation.Count; i++)
                        {
                            Type type = typeof(PatientExtention);
                            var info = type.GetProperties().ExceptKey().ExcepteIgnoreProperty().ToList();
                            for (int j = 1; j <= 16; j++)
                            {
                                workSheet.Cells[i + 2, j].Value = info[j - 1].GetValue(PatientsInformation[i]);
                            }
                        }

                        //日期格式转换
                        workSheet.Cells[$"N2:N{PatientsInformation.Count + 1}"].Style.Numberformat.Format = "yyyy-MM-dd";
                        workSheet.Cells[$"O2:O{PatientsInformation.Count + 1}"].Style.Numberformat.Format = "yyyy-MM-dd";
                        workSheet.Cells[$"P2:P{PatientsInformation.Count + 1}"].Style.Numberformat.Format = "yyyy-MM-dd";

                        workSheet.Cells.AutoFitColumns();
                        workSheet.View.PageLayoutView = true;
                        var xlsxFile = FileOutputUtil.GetFileInfo(saveFileDialog.SafeFileName);
                        package.SaveAs(xlsxFile);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
        }

        #endregion

        #region 诊断



        #endregion

        /// <summary>
        /// 刷新
        /// </summary>
        protected async void Refresh()
        {         
            ObservableCollection<PatientExtention> tsNew = new ObservableCollection<PatientExtention>();
            await Task.Factory.StartNew(() =>
            {
                SelectedCount = 0;
                tsNew = GetPatientData(); //刷新后不记住选择
                //tsNew = PatientsInformation; //刷新后记住选择
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
        public void ShowWindow<T>(object ViewModel, FrameworkElement frameworkElement) where T : Window
        {
            Type type = typeof(T);
            T instance = (T)Activator.CreateInstance(type);
            instance.DataContext = ViewModel;
            instance.Owner = Window.GetWindow(frameworkElement) as MedicalReportWindow;
            instance.Show();
        }

        /// <summary>
        /// 异步更新UI
        /// </summary>
        /// <param name="action"></param>
        public static void Action(Action action)
        {
            App.Current.Dispatcher.BeginInvoke(action = () => { });
        }

        /// <summary>
        /// 外网访问
        /// </summary>
        private void GitHyperink(Hyperlink hyperlink)
        {
            Process.Start(new ProcessStartInfo("explorer", hyperlink.NavigateUri.AbsoluteUri));
        }

        /// <summary>
        /// 得到与bin文件同级的目录
        /// </summary>
        /// <returns></returns>
        public static string GetBinPath()
        {
            var path = Directory.GetCurrentDirectory();
            path = Directory.GetParent(path).Parent.Parent.FullName;
            return path;
        }
    }
}
