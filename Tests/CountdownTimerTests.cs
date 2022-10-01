using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FakeItEasy;
using NUnit.Framework;
using Ponydoro_Common;

namespace Tests
{
    [TestFixture]
    public class CountdownTimerTests
    {
        [SetUp]
        public void SetUp()
        {
            _fakeTimerFacade = A.Fake<ITimerFacade>();
            _fakeSoundProvider = A.Fake<ISoundProvider>();

            _sut = new CountdownTimer(_fakeTimerFacade, _fakeSoundProvider) { Callback = Callback };

            _callBackList = new List<TimeSpan>();
            _timerFacadeCallback = null;

            A.CallTo(() => _fakeTimerFacade.NewTimer(A<TimerCallback>.Ignored)).Invokes(a =>
            {
                _timerFacadeCallback = a.GetArgument<TimerCallback>(0);
            });
        }

        private CountdownTimer _sut;
        private List<TimeSpan> _callBackList;
        private ITimerFacade _fakeTimerFacade;
        private ISoundProvider _fakeSoundProvider;
        private TimerCallback _timerFacadeCallback;

        private void Callback(TimeSpan obj)
        {
            _callBackList.Add(obj);
        }

        [TestCase(0, 0)]
        [TestCase(15, 0.25)]
        [TestCase(30, 0.5)]
        [TestCase(45, 0.75)]
        [TestCase(60, 1)]
        [TestCase(88, 1)]
        public void PercentageComplete(int elapsedSeconds, double expectedPercentage)
        {
            _sut.StartCountdown(1, false);

            while (_callBackList.Last().TotalSeconds > elapsedSeconds)
                _timerFacadeCallback.Invoke(null);

            Assert.AreEqual(expectedPercentage, _sut.PercentageToGo);
        }

        [Test]
        public void Countdown_end_triggers_noise_when_enabled()
        {
            _sut.StartCountdown(1, true);

            for (var i = 0; i < 59; i++)
                _timerFacadeCallback.Invoke(null);

            A.CallTo(() => _fakeSoundProvider.MakeSound()).MustNotHaveHappened();
            _timerFacadeCallback?.Invoke(null);
            A.CallTo(() => _fakeSoundProvider.MakeSound()).MustHaveHappened();
        }

        [Test]
        public void Countdown_end_does_not_trigger_noise_when_disabled()
        {
            _sut.StartCountdown(1, false);

            for (var i = 0; i < 59; i++)
                _timerFacadeCallback.Invoke(null);

            A.CallTo(() => _fakeSoundProvider.MakeSound()).MustNotHaveHappened();
            _timerFacadeCallback?.Invoke(null);
            A.CallTo(() => _fakeSoundProvider.MakeSound()).MustNotHaveHappened();
        }

        [Test]
        public void Countdown_triggers_callback_every_update()
        {
            _sut.StartCountdown(1, false);

            for (var i = 0; i < 59; i++)
                _timerFacadeCallback.Invoke(null);

            Assert.AreEqual(60, _callBackList.Count); //set to 60, count to 1, makes 60 total changes
            var changes = Enumerable.Range(1, 60).Reverse().ToList();
            CollectionAssert.AreEquivalent(changes, _callBackList.Select(timeSpan => timeSpan.TotalSeconds));

            _timerFacadeCallback?.Invoke(null);

            changes = changes.Append(0).ToList();
            CollectionAssert.AreEquivalent(changes, _callBackList.Select(timeSpan => timeSpan.TotalSeconds));
        }

        [Test]
        public void StartCountdown_triggers_timer()
        {
            const int minutes = 10;
            _sut.StartCountdown(minutes, false);

            A.CallTo(() => _fakeTimerFacade.KillTimer()).MustHaveHappened()
                .Then(A.CallTo(() => _fakeTimerFacade.NewTimer(A<TimerCallback>.Ignored)).MustHaveHappened());
            Assert.AreEqual(minutes, _callBackList.Last().TotalMinutes);
        }

        [Test]
        public void StopCountdown_stops_timer()
        {
            _sut.StartCountdown(1, true);
            _sut.StopCountdown();

            A.CallTo(() => _fakeTimerFacade.KillTimer()).MustHaveHappened();
            Assert.AreEqual(0, _callBackList.Last().TotalSeconds);
        }
    }
}