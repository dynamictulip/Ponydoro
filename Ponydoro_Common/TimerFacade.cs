using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Pomo_Shiny
{
    public interface ITimerFacade
    {
        void NewTimer(TimerCallback updateRemainingTime);
        void KillTimer();
    }

    [ExcludeFromCodeCoverage]
    public class TimerFacade : ITimerFacade
    {
        private Timer _timer;

        public void NewTimer(TimerCallback updateRemainingTime)
        {
            _timer = new Timer(updateRemainingTime, null, 1000, 1000);
        }

        public void KillTimer()
        {
            _timer?.Dispose();
        }
    }
}