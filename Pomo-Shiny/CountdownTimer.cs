using System;

namespace Pomo_Shiny
{
    public interface ICountdownTimer
    {
        Action<TimeSpan> Callback { get; set; }
        double PercentageToGo { get; }
        void StartCountdown(int minutes);
        void StopCountdown();
    }

    public class CountdownTimer : ICountdownTimer
    {
        private readonly ISoundProvider _soundProvider;
        private readonly ITimerFacade _timerFacade;
        private int _countdownStartMinutes;
        private TimeSpan _remainingTime;

        public CountdownTimer(ITimerFacade timerFacade, ISoundProvider soundProvider)
        {
            _timerFacade = timerFacade;
            _soundProvider = soundProvider;
        }

        private TimeSpan RemainingTime
        {
            get => _remainingTime;
            set
            {
                _remainingTime = value;
                Callback(value);
            }
        }

        public double PercentageToGo =>
            _countdownStartMinutes == 0 ? 1 : RemainingTime.TotalMinutes / _countdownStartMinutes;

        public void StartCountdown(int minutes)
        {
            StopCountdown();
            _countdownStartMinutes = minutes;
            RemainingTime = new TimeSpan(0, 0, minutes, 0);
            //     RemainingTime = new TimeSpan(0, 0, 0, 5);
            _timerFacade.NewTimer(UpdateRemainingTime);
        }

        public void StopCountdown()
        {
            _timerFacade.KillTimer();
            RemainingTime = new TimeSpan(0, 0, 0, 0);
        }

        public Action<TimeSpan> Callback { get; set; }

        private void UpdateRemainingTime(object state)
        {
            RemainingTime = RemainingTime.Subtract(new TimeSpan(0, 0, 0, 1));
            if (RemainingTime.TotalSeconds < 1)
            {
                _soundProvider.MakeSound();
                StopCountdown();
            }
        }
    }
}