using System;
using System.Threading;
using System.Windows.Input;

namespace Pomo_Shiny
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IApplicationAccessor _applicationAccessor;
        private TimeSpan _remainingTime;
        private Timer _timer;

        public MainWindowViewModel(IApplicationAccessor applicationAccessor)
        {
            _applicationAccessor = applicationAccessor;

            ExitCommand = new DelegateCommand(Exit);
            StartTimerCommand = new DelegateCommand(StartTimer);
            StartBreakTimerCommand = new DelegateCommand(StartBreakTimer);
        }

        public ICommand ExitCommand { get; }
        public ICommand StartTimerCommand { get; }
        public ICommand StartBreakTimerCommand { get; }

        public TimeSpan RemainingTime
        {
            get => _remainingTime;
            private set
            {
                _remainingTime = value;
                RaisePropertyChangedEvent(nameof(RemainingTime));
            }
        }

        private void StartTimer()
        {
            NewTimer(25);
        }

        private void StartBreakTimer()
        {
            NewTimer(5);
        }

        private void NewTimer(int minutes)
        {
            _timer?.Dispose();
            RemainingTime = new TimeSpan(0, 0, minutes, 0);
            _timer = new Timer(UpdateRemainingTime, null, 1000, 1000);
        }

        private void UpdateRemainingTime(object state)
        {
            RemainingTime = RemainingTime.Subtract(new TimeSpan(0, 0, 0, 1));
            if (RemainingTime.TotalSeconds < 1)
                _timer.Dispose();
        }

        private void Exit()
        {
            _applicationAccessor.Shutdown();
        }
    }
}