using System.Collections.Generic;

namespace YourVitebskApp.Models
{
    public class VoatByRouteAttribute
    {
        public string napr { get; set; }
        public string napr_name { get; set; }
        public string tr_n { get; set; }
        public IEnumerable<VoatByTransportStop> ost { get; set; }
    }
}
