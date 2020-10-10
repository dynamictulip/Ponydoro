using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Ponydoro_WPF
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