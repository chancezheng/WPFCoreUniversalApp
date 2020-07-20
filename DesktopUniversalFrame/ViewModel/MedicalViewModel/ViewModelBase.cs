using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopUniversalFrame.ViewModel.MedicalViewModel
{
    public class ViewModelBase : WindowCommandBaseModel
    {
        private MedicalReportViewModel MedicalReportViewModel { get; set; }
        private DiagnoseViewModel DiagnoseViewModel { get; set; }

        public ViewModelBase()
        {
            MedicalReportViewModel = new MedicalReportViewModel();
            DiagnoseViewModel = new DiagnoseViewModel();
            MedicalReportViewModel.MessengerViewModelDelegate += DiagnoseViewModel.MessengerViewModelDelegate;
        }
    }
}
