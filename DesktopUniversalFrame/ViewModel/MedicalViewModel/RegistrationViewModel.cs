using DesktopUniversalCustomControl.CustomView.MsgDlg;
using DesktopUniversalFrame.Entity;
using DesktopUniversalFrame.Entity.CustomValidationRules;
using DesktopUniversalFrame.Model.MedicalModel;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DesktopUniversalFrame.ViewModel.MedicalViewModel
{
    public class RegistrationViewModel: MedicalReportViewModel, IValidationExceptionHandle
    {
        private bool _hasValidationError = false;
        private PatientExtention _patientRegister;
        private bool _isUIVisible;


        /// <summary>
        /// 验证有无错误
        /// </summary>
        public bool HasValidationError
        {
            get => _hasValidationError;
            set => SetProperty(ref _hasValidationError, value);
        }
        
        /// <summary>
        /// 每个病人
        /// </summary>
        public PatientExtention PatientRegister
        {
            get => _patientRegister;
            set => SetProperty(ref _patientRegister, value);
        }
     
        /// <summary>
        /// 提交按钮是否显示
        /// </summary>
        public bool IsUIVisible
        {
            get =>_isUIVisible;
            set => SetProperty(ref _isUIVisible, value);
        }


        private DelegateCommand<Button> _submitPatientInfoCommand;
        public DelegateCommand<Button> SubmitPatientInfoCommand
        {
            get { return _submitPatientInfoCommand; }
            set { _submitPatientInfoCommand = value; }
        }



        public RegistrationViewModel()
        {
            LoadedWindowCommand = new DelegateCommand<Window>(Loaded);
            SubmitPatientInfoCommand = new DelegateCommand<Button>(SubmitPatientInfo);
        }

        private void SubmitPatientInfo(Button btn)
        {
            bool isInsert = ORMHelper.InsertData(PatientRegister);           
            if (isInsert)
            {
                Action(Refresh);
                //MessageDialog.Show("成功插入一条数据");
            }
            else
                MessageDialog.Show("Failed");
        }

        private void Loaded(Window win)
        {
            var ss = ("Bob", 2);
        }
    }
}
