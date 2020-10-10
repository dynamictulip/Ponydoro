using System;
using System.Windows.Input;
using Microsoft.Extensions.Options;
using Ponydoro.Mvvm;
using Ponydoro_Common;

namespace Ponydoro
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IApplicationAccessor _applicationAccessor;
        private readonly ICountdownTimer _countdownTimer;
        private readonly AppSettings _appSettings;
        private TimeSpan _remainingTime;

        public MainWindowViewModel(IApplicationAccessor applicationAccessor, ICountdownTimer countdownTimer, IOptions<AppSettings> appSettings)
        {
            _applicationAccessor = applicationAccessor;
            _countdownTimer = countdownTimer;
            _appSettings = appSettings.Value;
            _countdownTimer.Callback = val => RemainingTime = val;

            ExitCommand = new DelegateCommand(Exit);
            StartTimerCommand = new DelegateCommand(StartTimer);
            StartBreakTimerCommand = new DelegateCommand(StartBreakTimer);
            StopTimerCommand = new DelegateCommand(StopTimer);
        }

        public ICommand ExitCommand { get; }
        public ICommand StartTimerCommand { get; }
        public ICommand StartBreakTimerCommand { get; }
        public ICommand StopTimerCommand { get; }

        public TimeSpan RemainingTime
        {
            get => _remainingTime;
            private set
            {
                _remainingTime = value;
                RaisePropertyChangedEvent(nameof(RemainingTime));
                RaisePropertyChangedEvent(nameof(TimerOff));
                RaisePropertyChangedEvent(nameof(PercentageToGo));
                RaisePropertyChangedEvent(nameof(Title));
            }
        }

        public bool TimerOff => RemainingTime.TotalSeconds < 1;
        public double PercentageToGo => _countdownTimer.PercentageToGo;

        public string Title => PercentageToGo >= 1 || PercentageToGo <= 0 ? "Ready for a ponydorro?" :
            PercentageToGo > 0.5 ? "Giddy up" :
            PercentageToGo > 0.1 ? "Keep trotting" : "Nearly there";

        private void StartTimer()
        {
            _countdownTimer.StartCountdown(25, _appSettings.IsSoundOn);
        }

        private void StopTimer()
        {
            _countdownTimer.StopCountdown();
        }

        private void StartBreakTimer()
        {
            _countdownTimer.StartCountdown(5, _appSettings.IsSoundOn);
        }

        private void Exit()
        {
            _applicationAccessor.Shutdown();
        }
    }
}