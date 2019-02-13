using System;
using System.Threading;

namespace Pomo_Shiny
{
    public interface ICountdownTimer
    {
        Action<TimeSpan> Callback { get; set; }
        void StartCountdown(int minutes);
    }

    internal class CountdownTimer : ICountdownTimer
    {
        private TimeSpan _remainingTime;
        private Timer _timer;

        private TimeSpan RemainingTime
        {
            get => _remainingTime;
            set
            {
                _remainingTime = value;
                Callback(value);
            }
        }

        public void StartCountdown(int minutes)
        {
            _timer?.Dispose();
            RemainingTime = new TimeSpan(0, 0, minutes, 0);
            _timer = new Timer(UpdateRemainingTime, null, 1000, 1000);
        }

        public Action<TimeSpan> Callback { get; set; }

        private void UpdateRemainingTime(object state)
        {
            RemainingTime = RemainingTime.Subtract(new TimeSpan(0, 0, 0, 1));
            if (RemainingTime.TotalSeconds < 1)
                _timer.Dispose();
        }
    }
}