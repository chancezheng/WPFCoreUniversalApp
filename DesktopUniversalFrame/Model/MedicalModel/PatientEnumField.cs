using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopUniversalFrame.Model.MedicalModel
{
    /// <summary>
    /// 诊断状态
    /// </summary>
    public enum DiagnoseState
    {
        /// <summary>
        /// 未诊断
        /// </summary>
        Undiagnose,

        /// <summary>
        /// 已诊断，未复核
        /// </summary>
        Diagnosed,

        /// <summary>
        /// 已复核
        /// </summary>
        Reviewered
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        Male,
        Female,
        Unknow,
    }
}
