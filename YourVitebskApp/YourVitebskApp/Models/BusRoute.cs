using System.Collections.Generic;

namespace YourVitebskApp.Models
{
    public class BusRoute
    {
        public int BusId { get; set; }
        public string BusNumber { get; set; }
        public int BusRouteNumber { get; set; }
        public string BusRouteName { get; set; }
        public IEnumerable<BusStop> BusStops { get; set; }
    }
}
