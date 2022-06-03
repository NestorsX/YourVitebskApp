using System.Drawing;

namespace YourVitebskApp.Helpers
{
    public interface IEnvironment
    {
        void SetStatusBarColorAsync(Color color, bool darkStatusBarTint);
    }
}
