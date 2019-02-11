using FakeItEasy;
using NUnit.Framework;
using Pomo_Shiny;

namespace Tests
{
    public class MainWindowViewModelTests
    {
        private MainWindowViewModel sut;
        private IApplicationAccessor _fakeApplicationAccessor;

        [SetUp]
        public void Setup()
        {
            _fakeApplicationAccessor = A.Fake<IApplicationAccessor>();
            sut = new MainWindowViewModel(_fakeApplicationAccessor);
        }

        [Test]
        public void ExitCommandExits()
        {
            sut.ExitCommand.Execute(null);

            A.CallTo(() => _fakeApplicationAccessor.Shutdown()).MustHaveHappened();
        }
    }
}