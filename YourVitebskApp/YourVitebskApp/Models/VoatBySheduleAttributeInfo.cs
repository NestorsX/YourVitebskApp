using System.Collections.Generic;

namespace YourVitebskApp.Models
{
    public class VoatBySheduleAttributeInfo
    {
        public string info_marsh { get; set; }
        public string spr_ostanovki_name { get; set; }
        public string spr_ostanovki_id { get; set; }
        public string vid_transp { get; set; }
        public string spr_vid_marsch_name { get; set; }
        public string spr_marsh_napr { get; set; }
        public string transp_name { get; set; }
        public string spr_marsh_napr_ost_id { get; set; }
        public string napr_n { get; set; }
        public IEnumerable<VoatBySheduleTime> times { get; set; }
    }
}
