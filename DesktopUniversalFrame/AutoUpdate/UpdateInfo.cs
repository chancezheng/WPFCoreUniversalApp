using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopUniversalFrame.AutoUpdate
{
    public class UpdateInfo
    {
        public string AppName { get; set; }

        public Version AppVersion { get; set; }

        public Version RequiredMinVersion { get; set; }

        public string Description { get; set; }

        public Guid MD5 { get; set; }
    }
}
