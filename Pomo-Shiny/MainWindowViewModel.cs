using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;

namespace Pomo_Shiny
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IApplicationAccessor _applicationAccessor;
        private Timer timer;

        public MainWindowViewModel(IApplicationAccessor applicationAccessor)
        {
            _applicationAccessor = applicationAccessor;

            ExitCommand = new DelegateCommand(Exit);
            StartTimerCommand = new DelegateCommand(StartTimer);
        }

        private void StartTimer()
        {
            timer = new Timer(ElapsedTime);

        }

        private void ElapsedTime(object state)
        {
            throw new System.NotImplementedException();
        }


        public ICommand ExitCommand { get; }

        public ICommand StartTimerCommand { get; }

        private void Exit()
        {
            _applicationAccessor.Shutdown();
        }
    }
}