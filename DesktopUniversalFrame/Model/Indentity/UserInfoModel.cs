using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text;

namespace DesktopUniversalFrame.Model.Indentity
{
    public class UserInfoModel : BaseModel
    {
        [StringLength(maximumLength: 20, MinimumLength = 0)]
        public string userName { get; set; }

        [StringLength(16)]
        public string Password { get; set; }

        public AuthorityModel Authority { get; set; }

        [Phone]
        public string phoneNumber { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
