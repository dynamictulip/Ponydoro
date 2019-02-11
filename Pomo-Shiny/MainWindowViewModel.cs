using System.Windows.Input;

namespace Pomo_Shiny
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IApplicationAccessor _applicationAccessor;

        public MainWindowViewModel(IApplicationAccessor applicationAccessor)
        {
            _applicationAccessor = applicationAccessor;
        }

        public ICommand ExitCommand => new DelegateCommand(Exit);

        private void Exit()
        {
            _applicationAccessor.Shutdown();
        }
    }
}