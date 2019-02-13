using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Pomo_Shiny
{
    public interface ICountdownTimer
    {
        Action<TimeSpan> Callback { get; set; }
        void StartCountdown(int minutes);
        void StopCountdown();
    }

    [ExcludeFromCodeCoverage] //hard to test timer
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
            StopCountdown();
            RemainingTime = new TimeSpan(0, 0, minutes, 0);
            _timer = new Timer(UpdateRemainingTime, null, 1000, 1000);
        }

        public void StopCountdown()
        {
            _timer?.Dispose();
            RemainingTime = new TimeSpan(0, 0, 0, 0);
        }

        public Action<TimeSpan> Callback { get; set; }

        private void UpdateRemainingTime(object state)
        {
            RemainingTime = RemainingTime.Subtract(new TimeSpan(0, 0, 0, 1));
            if (RemainingTime.TotalSeconds < 1)
                StopCountdown();
        }
    }
}