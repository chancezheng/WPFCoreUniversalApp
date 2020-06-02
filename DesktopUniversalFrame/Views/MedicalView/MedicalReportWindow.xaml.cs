using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopUniversalFrame.Views.MedicalView
{
    /// <summary>
    /// MedicalMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MedicalReportWindow : Window
    {
        public MedicalReportWindow()
        {
            InitializeComponent();
        }

        private void Sample2_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("SAMPLE 2: Closing dialog with parameter: " + (eventArgs.Parameter ?? ""));
        }
    }

    /// <summary>
    /// 报告系统功能
    /// </summary>
    public class ReportFunctionInfo
    {       
        public string Function { get; set; }

        public List<FunctionItems> FunctionItems { get; set; }
    }

    /// <summary>
    /// 挂号功能
    /// </summary>
    public class FunctionItems
    {
        public PackIconKind Icon { get; set; }
        public string Operation { get; set; }
    }
}
