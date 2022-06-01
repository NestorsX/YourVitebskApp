using System.Collections.Generic;

namespace YourVitebskApp.Models
{
    public class VoatByContent<T>
    {
        public IEnumerable<T> data { get; set; }
    }
}
