using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopUniversalFrame.Model.Indentity
{
    /// <summary>
    /// 权限类别
    /// </summary>
    public enum AuthorityModel
    {
        /// <summary>
        /// 管理员(最高权限)
        /// </summary>
        Admin,
        /// <summary>
        /// Vip
        /// </summary>
        Vip,
        /// <summary>
        /// 普通用户
        /// </summary>
        General,
        /// <summary>
        /// 参观者
        /// </summary>
        Vistor,  
    }
}
