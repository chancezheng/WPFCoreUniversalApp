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
        public string Id { get; set; }

    }
}
