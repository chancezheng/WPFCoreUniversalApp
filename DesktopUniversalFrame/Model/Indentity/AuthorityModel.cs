using DesktopUniversalFrame.Common.MappingAttribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopUniversalFrame.Model.Indentity
{
    /// <summary>
    /// 权限类别
    /// </summary>
    [TableMapping("authority")]
    public class AuthorityModel
    {
        /// <summary>
        /// 管理员(最高权限)
        /// </summary>
        public string Admin { get; set; }

        /// <summary>
        /// Vip
        /// </summary>
        public string Vip { get; set; }

        /// <summary>
        /// 普通用户
        /// </summary>
        public string General { get; set; }

        /// <summary>
        /// 参观者
        /// </summary>
        public string Vistor { get; set; }
    }
}
