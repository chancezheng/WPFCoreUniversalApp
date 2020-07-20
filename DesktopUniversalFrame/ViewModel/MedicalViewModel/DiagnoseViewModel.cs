using DesktopUniversalCustomControl.CustomComponent;
using DesktopUniversalFrame.Model.MedicalModel;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace DesktopUniversalFrame.ViewModel.MedicalViewModel
{ 
    public class DiagnoseViewModel : WindowCommandBaseModel
    {       
        public List<ImageItemInfo> ImageItems { get; set; }
        private static List<PatientExtention> Patients;

        private FlowDocument fdoc;
        private PatientExtention _patient;
        private IDocumentPaginatorSource _flowDocument;
        private int _imageIndex;


        public PatientExtention Patient
        {
            get => _patient;
            set => SetProperty(ref _patient, value);
        }

        public IDocumentPaginatorSource FlowDocument
        {
            get => _flowDocument;
            set => SetProperty(ref _flowDocument, value);
        }
        
        public int ImageIndex
        {
            get => _imageIndex;
            set 
            {
                if (_imageIndex == value) return;               
                SetProperty(ref _imageIndex, value);
                Patient = Patients[value];
            }
        }


        public DiagnoseViewModel()
        {
            string baseUri = AppDomain.CurrentDomain.BaseDirectory;
            List<string> list1 = GetImages().ToList();
            List<string> list2 = new List<string>() { "Title1", "Title2", "Title3", "Title4", "Title5" };
            List<string> list3 = new List<string>()
            {  baseUri + "WholeScene\\first\\", baseUri + "WholeScene\\first\\", baseUri + "WholeScene\\first\\",
                baseUri + "WholeScene\\first\\", baseUri + "WholeScene\\first\\" };

            ImageItems = new List<ImageItemInfo>();
            for (int i = 0; i < 5; i++)
            {
                ImageItems.Add(new ImageItemInfo
                {
                    imgUri = list1[i],
                    title = list2[i],
                    scenePath = list3[i],
                });
            }

            LoadedWindowCommand = new DelegateCommand<Window>(Loaded);
        }

        private void Loaded(Window win)
        {
            GetImages();
            GetFlowDocument();
            Application.Current.Dispatcher.BeginInvoke(new Action(() => LoadXps()), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        private string[] GetImages()
        {
            var files = Directory.GetFiles(MedicalReportViewModel.GetBinPath() + "\\Resource\\Medical\\Images\\","xindian*");
            return files;
        }

        private void GetFlowDocument()
        {
            fdoc = (FlowDocument)Application.LoadComponent(new Uri("/DesktopUniversalFrame;component/Views/MedicalView/PatientInfoDocument.xaml", UriKind.RelativeOrAbsolute));
            fdoc.PagePadding = new Thickness(20);
            fdoc.DataContext = Patient;
        }

        private void LoadXps()
        {
            MemoryStream ms = new MemoryStream();
            Package package = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);
            Uri documentUri = new Uri("pack://InMemoryDocment.xps");
            PackageStore.RemovePackage(documentUri);
            PackageStore.AddPackage(documentUri, package);
            XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.Fast, documentUri.AbsoluteUri);

            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
            writer.Write(((IDocumentPaginatorSource)fdoc).DocumentPaginator);
            //获取这个基于内存的xps document的fixed document
            FlowDocument = xpsDocument.GetFixedDocumentSequence();

            //关闭基于内存的xps document
            xpsDocument.Close();
        }

        //委托接受消息
        public void MessengerViewModelDelegate(List<PatientExtention> patients)
        {
            Patients = new List<PatientExtention>();
            Patients = patients;
        }
    }
}
