using System;
using FakeItEasy;
using NUnit.Framework;
using Pomo_Shiny;

namespace Tests
{
    public class MainWindowViewModelTests
    {
        private MainWindowViewModel _sut;
        private IApplicationAccessor _fakeApplicationAccessor;
        private ICountdownTimer _fakeCountdownTimer;

        [SetUp]
        public void Setup()
        {
            _fakeApplicationAccessor = A.Fake<IApplicationAccessor>();
            _fakeCountdownTimer = A.Fake<ICountdownTimer>();
            _sut = new MainWindowViewModel(_fakeApplicationAccessor, _fakeCountdownTimer);
        }

        [Test]
        public void ExitCommand_exits()
        {
            _sut.ExitCommand.Execute(null);

            A.CallTo(() => _fakeApplicationAccessor.Shutdown()).MustHaveHappened();
        }

        [Test]
        public void StartTimerCommand_starts_timer_for_25_minutes()
        {
            _sut.StartTimerCommand.Execute(null);

            A.CallTo(() => _fakeCountdownTimer.StartCountdown(25)).MustHaveHappened();
        }

        [Test]
        public void StartBreakTimerCommand_starts_timer_for_5_minutes()
        {
            _sut.StartBreakTimerCommand.Execute(null);

            A.CallTo(() => _fakeCountdownTimer.StartCountdown(5)).MustHaveHappened();
        }

        [Test]
        public void Change_in_timer_updates_RemainingTime_in_viewmodel()
        {
            Assert.AreEqual(0, _sut.RemainingTime.TotalSeconds);

            _fakeCountdownTimer.Callback(TimeSpan.MaxValue);
            Assert.AreEqual(TimeSpan.MaxValue, _sut.RemainingTime);

            _fakeCountdownTimer.Callback(TimeSpan.MinValue);
            Assert.AreEqual(TimeSpan.MinValue, _sut.RemainingTime);

            var timeSpan = new TimeSpan(0,0,23,22);
            _fakeCountdownTimer.Callback(timeSpan);
            Assert.AreEqual(timeSpan, _sut.RemainingTime);
        }
    }
}