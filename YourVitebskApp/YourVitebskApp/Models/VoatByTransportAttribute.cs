using System.Collections.Generic;

namespace YourVitebskApp.Models
{
    public class VoatByTransportAttribute
    {
        public string vid_tr { get; set; }
        public string vid_tr_n { get; set; }
        public IEnumerable<VoatByTransportItem> transpes { get; set; }
    }
}
