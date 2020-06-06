using DesktopUniversalFrame.Common;
using DesktopUniversalFrame.Common.MappingAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DesktopUniversalFrame.Model.MedicalModel
{
    /// <summary>
    /// Patient扩展类
    /// 数据库交互时忽略此类
    /// </summary>   
    public class PatientExtention : Patient, INotifyPropertyChanged
    {
        //[IgnoreSomeProperty("Ignore")]
        private bool _isSelected;
        private int _indexOfItem;


        /// <summary>
        /// 是否选中
        /// </summary>
        [IgnoreSomeProperty("Ignore")]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// 子项序号
        /// </summary>       
        [IgnoreSomeProperty("Ignore")]
        public int IndexOfItem
        {
            get => _indexOfItem;
            set 
            {
                if (_indexOfItem == value) return;
                _indexOfItem = value;
                OnPropertyChanged();
            } 
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
