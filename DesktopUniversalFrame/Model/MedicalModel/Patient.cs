using DesktopUniversalFrame.Common.MappingAttribute;
using DesktopUniversalFrame.Common.ValueConverter;
using DesktopUniversalFrame.Model.Indentity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace DesktopUniversalFrame.Model.MedicalModel
{
    /// <summary>
    /// 病人信息
    /// </summary>
    //[TableMapping("patientinfo")]
    [Table("patientinfo")]
    [Serializable]
    public class Patient : BaseModel
    {
        public string PatientName { get; set; }

        public int Age { get; set; }

        [TypeConverter(typeof(GenderIntConverter))]
        public Gender Gender { get; set; }

        public string PhoneNumber { get; set; }

        /// <summary>
        /// 样本号
        /// </summary>
        public string SampleNumber { get; set; }

        /// <summary>
        /// 样本条码号
        /// </summary>
        public string SampleBarCode { get; set; }

        /// <summary>
        /// 样本类型
        /// </summary>
        public string SampleType { get; set; }

        /// <summary>
        /// 报告类型
        /// </summary>
        public string ReportType { get; set; }

        /// <summary>
        /// 诊断状态
        /// </summary>
        [TypeConverter(typeof(EnumToDiagnoseStateConverter))]
        public DiagnoseState DiagnoseState { get; set; }

        /// <summary>
        /// 送检医院
        /// </summary>
        public string InspectHospital { get; set; }

        /// <summary>
        /// 送检科室
        /// </summary>
        public string InspectDepartment { get; set; }

        /// <summary>
        /// 送检医生
        /// </summary>
        public string InspectDoctor { get; set; }

        /// <summary>
        /// 门诊号
        /// </summary>
        public string OutDepartment { get; set; }

        /// <summary>
        /// 挂号日期
        /// </summary>
        public DateTime RegisterTime { get => DateTime.Now; set => value = DateTime.Now; }

        /// <summary>
        /// 导出日期
        /// </summary>
        public DateTime ExportTime { get => DateTime.Now; set => value = DateTime.Now; }

        /// <summary>
        /// 打印日期
        /// </summary>
        public DateTime PrintTime { get => DateTime.Now; set => value = DateTime.Now; }
    }
}
