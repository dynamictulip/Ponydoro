using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FakeItEasy;
using NUnit.Framework;
using Pomo_Shiny;

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

            _sut = new CountdownTimer(_fakeTimerFacade, _fakeSoundProvider) {Callback = Callback};

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

        [Test]
        public void Countdown_end_triggers_noise()
        {
            _sut.StartCountdown(1);

            for (var i = 0; i < 59; i++)
                _timerFacadeCallback.Invoke(null);

            A.CallTo(() => _fakeSoundProvider.MakeSound()).MustNotHaveHappened();
            _timerFacadeCallback?.Invoke(null);
            A.CallTo(() => _fakeSoundProvider.MakeSound()).MustHaveHappened();
        }

        [Test]
        public void Countdown_triggers_callback_every_update()
        {
            _sut.StartCountdown(1);

            for (var i = 0; i < 59; i++)
                _timerFacadeCallback.Invoke(null);

            Assert.AreEqual(61, _callBackList.Count); //set to 0, then to 60, then 59 changes makes 61 total changes
            var changes = Enumerable.Range(1, 60).Reverse().Prepend(0).ToList();
            CollectionAssert.AreEquivalent(changes, _callBackList.Select(timeSpan => timeSpan.TotalSeconds));


            _timerFacadeCallback?.Invoke(null);

            changes = changes.Append(0).Append(0).ToList();
            CollectionAssert.AreEquivalent(changes, _callBackList.Select(timeSpan => timeSpan.TotalSeconds));
        }

        [Test]
        public void StartCountdown_triggers_timer()
        {
            const int minutes = 10;
            _sut.StartCountdown(minutes);

            A.CallTo(() => _fakeTimerFacade.KillTimer()).MustHaveHappened()
                .Then(A.CallTo(() => _fakeTimerFacade.NewTimer(A<TimerCallback>.Ignored)).MustHaveHappened());
            Assert.AreEqual(minutes, _callBackList.Last().TotalMinutes);
        }

        [Test]
        public void StopCountdown_stops_timer()
        {
            _sut.StopCountdown();

            A.CallTo(() => _fakeTimerFacade.KillTimer()).MustHaveHappened();
            Assert.AreEqual(0, _callBackList.Last().TotalSeconds);
        }
    }
}