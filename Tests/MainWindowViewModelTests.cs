using System;
using FakeItEasy;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Ponydoro;
using Ponydoro_Common;

namespace Tests
{
    public class MainWindowViewModelTests
    {
        private IApplicationAccessor _fakeApplicationAccessor;
        private ICountdownTimer _fakeCountdownTimer;
        private MainWindowViewModel _sut;
        private IOptions<AppSettings> _fakeAppSettings;

        [SetUp]
        public void Setup()
        {
            _fakeApplicationAccessor = A.Fake<IApplicationAccessor>();
            _fakeCountdownTimer = A.Fake<ICountdownTimer>();
            _fakeAppSettings = A.Fake<IOptions<AppSettings>>();

            _sut = new MainWindowViewModel(_fakeApplicationAccessor, _fakeCountdownTimer, _fakeAppSettings);
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

            A.CallTo(() => _fakeCountdownTimer.StartCountdown(25, A<bool>._)).MustHaveHappened();
        }

        [Test]
        public void StartBreakTimerCommand_starts_timer_for_5_minutes()
        {
            _sut.StartBreakTimerCommand.Execute(null);

            A.CallTo(() => _fakeCountdownTimer.StartCountdown(5, A<bool>._)).MustHaveHappened();
        }

        [Test]
        public void StopTimerCommand_stops_timer()
        {
            _sut.StopTimerCommand.Execute(null);

            A.CallTo(() => _fakeCountdownTimer.StopCountdown()).MustHaveHappened();
        }

        [Test]
        public void Change_in_timer_updates_RemainingTime_in_viewmodel()
        {
            Assert.AreEqual(0, _sut.RemainingTime.TotalSeconds);

            _fakeCountdownTimer.Callback(TimeSpan.MaxValue);
            Assert.AreEqual(TimeSpan.MaxValue, _sut.RemainingTime);

            _fakeCountdownTimer.Callback(TimeSpan.MinValue);
            Assert.AreEqual(TimeSpan.MinValue, _sut.RemainingTime);

            var timeSpan = new TimeSpan(0, 0, 23, 22);
            _fakeCountdownTimer.Callback(timeSpan);
            Assert.AreEqual(timeSpan, _sut.RemainingTime);
        }

        [TestCase(0, true)]
        [TestCase(-1, true)]
        [TestCase(1, false)]
        [TestCase(20, false)]
        [TestCase(2000000, false)]
        public void TimerOff_works(int secondsRemaining, bool expectedResult)
        {
            _fakeCountdownTimer.Callback(TimeSpan.FromSeconds(secondsRemaining));

            Assert.AreEqual(expectedResult, _sut.TimerOff);
        }

        [TestCase(0)]
        [TestCase(0.25)]
        [TestCase(0.56)]
        [TestCase(0.87)]
        [TestCase(1)]
        public void PercentageToGo_passes_value(double value)
        {
            A.CallTo(() => _fakeCountdownTimer.PercentageToGo).Returns(value);
            Assert.AreEqual(value, _sut.PercentageToGo);
        }

        [TestCase(1, "Ready for a ponydorro?")]
        [TestCase(0.9, "Giddy up")]
        [TestCase(0.75, "Giddy up")]
        [TestCase(0.49, "Keep trotting")]
        [TestCase(0.2, "Keep trotting")]
        [TestCase(0.01, "Nearly there")]
        [TestCase(0, "Ready for a ponydorro?")]
        public void Title_depends_on_PercentageToGo(double value, string expectedTitle)
        {
            A.CallTo(() => _fakeCountdownTimer.PercentageToGo).Returns(value);
            Assert.AreEqual(expectedTitle, _sut.Title);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void StartTimerCountdown_sound_controlled_by_config(bool soundIsOn)
        {
            _fakeAppSettings.Value.IsSoundOn = soundIsOn;

            _sut.StartTimerCommand.Execute(null);
            A.CallTo(() => _fakeCountdownTimer.StartCountdown(25, soundIsOn)).MustHaveHappened();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void StartBreakTimerCountdown_sound_controlled_by_config(bool soundIsOn)
        {
            _fakeAppSettings.Value.IsSoundOn = soundIsOn;

            _sut.StartBreakTimerCommand.Execute(null);
            A.CallTo(() => _fakeCountdownTimer.StartCountdown(5, soundIsOn)).MustHaveHappened();
        }
    }
}