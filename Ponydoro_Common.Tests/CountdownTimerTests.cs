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

            _remainingTime_CallBackList = new List<TimeSpan>();
            _timerFacadeCallback = null;

            A.CallTo(() => _fakeTimerFacade.NewTimer(A<TimerCallback>.Ignored)).Invokes(a =>
            {
                _timerFacadeCallback = a.GetArgument<TimerCallback>(0);
            });
        }

        private CountdownTimer _sut;
        private List<TimeSpan> _remainingTime_CallBackList;
        private ITimerFacade _fakeTimerFacade;
        private ISoundProvider _fakeSoundProvider;
        private TimerCallback _timerFacadeCallback;

        private void Callback(TimeSpan obj)
        {
            _remainingTime_CallBackList.Add(obj);
        }

        [TestCase(0, 0)]
        [TestCase(15, 0.25)]
        [TestCase(30, 0.5)]
        [TestCase(45, 0.75)]
        [TestCase(60, 1)]
        [TestCase(88, 1)]
        public void PercentageToGo_should_reflect_elapsed_time(int elapsedSeconds, double expectedPercentage)
        {
            _sut.StartCountdown(1, A.Dummy<bool>());

            while (_remainingTime_CallBackList.Last().TotalSeconds > elapsedSeconds)
                _timerFacadeCallback.Invoke(null);

            _sut.PercentageToGo.Should().Be(expectedPercentage);
        }

        [Test]
        public void Countdown_end_triggers_noise_when_enabled()
        {
            _sut.StartCountdown(1, true);

            //Ensure noise is not triggered when countdown is running
            for (var i = 0; i < 59; i++)
                _timerFacadeCallback.Invoke(null);
            A.CallTo(() => _fakeSoundProvider.MakeSound()).MustNotHaveHappened();

            //On last timer call, noise should trigger
            _timerFacadeCallback?.Invoke(null);
            A.CallTo(() => _fakeSoundProvider.MakeSound()).MustHaveHappened();
        }

        [Test]
        public void Countdown_end_does_not_trigger_noise_when_disabled()
        {
            _sut.StartCountdown(1, false);

            for (var i = 0; i < 60; i++)
                _timerFacadeCallback.Invoke(null);

            A.CallTo(() => _fakeSoundProvider.MakeSound()).MustNotHaveHappened();
        }

        [Test]
        public void Countdown_triggers_callback_every_update()
        {
            _sut.StartCountdown(1, A.Dummy<bool>());
            _remainingTime_CallBackList.Should().ContainSingle().Which.TotalSeconds.Should().Be(60);

            for (var i = 59; i >= 0; i--)
            {
                _timerFacadeCallback.Invoke(null);
                _remainingTime_CallBackList.Last().TotalSeconds.Should().Be(i);
            }
        }

        [Test]
        public void StartCountdown_kills_old_timer_then_starts_new_timer()
        {
            _sut.StartCountdown(10, A.Dummy<bool>());

            A.CallTo(() => _fakeTimerFacade.KillTimer()).MustHaveHappened()
                .Then(A.CallTo(() => _fakeTimerFacade.NewTimer(A<TimerCallback>.Ignored)).MustHaveHappened());
        }

        [TestCase(10)]
        [TestCase(25)]
        [TestCase(60)]
        public void StartCountdown_triggers_callback_with_correct_start_time(int minutes)
        {
            _sut.StartCountdown(minutes, A.Dummy<bool>());

            _remainingTime_CallBackList.Last().TotalMinutes.Should().Be(minutes);
        }

        [Test]
        public void StopCountdown_stops_timer()
        {
            _sut.StartCountdown(1, A.Dummy<bool>());
            _sut.StopCountdown();

            A.CallTo(() => _fakeTimerFacade.KillTimer()).MustHaveHappened();
        }

        [Test]
        public void StopCountdown_triggers_callback_when_timer_started()
        {
            _sut.StartCountdown(1, A.Dummy<bool>());
            _sut.StopCountdown();

            _remainingTime_CallBackList.Last().TotalSeconds.Should().Be(0);
        }

        [Test]
        public void StopCountdown_does_not_trigger_callback_when_timer_not_started()
        {
            _sut.StopCountdown();

            _remainingTime_CallBackList.Should().BeEmpty();
        }
    }
}