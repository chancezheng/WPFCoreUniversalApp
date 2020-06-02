using DesktopUniversalFrame.Common;
using DesktopUniversalFrame.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DesktopUniversalFrame.Model
{
    public abstract class BaseModel
    {
        [Key]
        public string id { get; set; }

    }

    /// <summary>
    /// 数据库操作类型
    /// </summary>
    public enum SqlOperationType
    {
        Select = 1,
        Insert = 2,
        Update = 3,
        Delete = 4,
    }
}
