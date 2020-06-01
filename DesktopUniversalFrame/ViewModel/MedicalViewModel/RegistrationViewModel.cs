using DesktopUniversalFrame.Entity.CustomValidationRules;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopUniversalFrame.ViewModel.MedicalViewModel
{
    public class RegistrationViewModel: WindowCommandBaseModel, IValidationExceptionHandle
    {
        private bool _hasValidationError = false;
        public bool HasValidationError
        {
            get => _hasValidationError;
            set => SetProperty(ref _hasValidationError, value);
        }

        #region ValidationData

        public string Name { get; set; }
        public string Age { get; set; }
        public string Phone { get; set; }

        #endregion
    }
}
