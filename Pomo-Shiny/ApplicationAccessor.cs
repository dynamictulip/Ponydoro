using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Pomo_Shiny
{
    public interface IApplicationAccessor
    {
        void Shutdown();
    }

    [ExcludeFromCodeCoverage]
    public class ApplicationAccessor : IApplicationAccessor
    {
        public void Shutdown()
        {
            Application.Current.Shutdown();
        }
    }
}