using System;

namespace Ponydoro_Common
{
    public interface ICountdownTimer
    {
        Action<TimeSpan> Callback { get; set; }
        double PercentageToGo { get; }
        void StartCountdown(int minutes, bool withSound);
        void StopCountdown();
    }

    public class CountdownTimer : ICountdownTimer
    {
        private readonly ISoundProvider _soundProvider;
        private readonly ITimerFacade _timerFacade;
        private int _countdownStartMinutes;
        private TimeSpan _remainingTime;
        private bool _makesound;

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
                if (_remainingTime == value)
                {
                    return;
                }
                _remainingTime = value;
                Callback(value);
            }
        }

        public double PercentageToGo =>
            _countdownStartMinutes == 0 ? 1 : RemainingTime.TotalMinutes / _countdownStartMinutes;

        public void StartCountdown(int minutes, bool withSound)
        {
            StopCountdown();

            _makesound = withSound;

            _countdownStartMinutes = minutes;
            RemainingTime = new TimeSpan(0, 0, minutes, 0);

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
                if (_makesound)
                    _soundProvider.MakeSound();
                StopCountdown();
            }
        }
    }
}