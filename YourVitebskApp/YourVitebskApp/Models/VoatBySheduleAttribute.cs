using System;
using System.Collections.Generic;
using System.Text;

namespace YourVitebskApp.Models
{
    public class VoatBySheduleAttribute
    {
        public string vid_transp { get; set; }
        public string napr_ost_id { get; set; }
        public IEnumerable<VoatBySheduleAttributeInfo> data_tr { get; set; }
    }
}
