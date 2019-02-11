using System.Windows.Input;

namespace Pomo_Shiny
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IApplicationAccessor _applicationAccessor;

        public MainWindowViewModel(IApplicationAccessor applicationAccessor)
        {
            _applicationAccessor = applicationAccessor;

            ExitCommand = new DelegateCommand(Exit);
        }

        public ICommand ExitCommand { get; }

        private void Exit()
        {
            _applicationAccessor.Shutdown();
        }
    }
}