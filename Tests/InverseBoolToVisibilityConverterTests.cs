using System;
using System.Globalization;
using System.Windows;
using NUnit.Framework;
using Ponydoro_WPF.Converters;

namespace Tests
{
    [TestFixture]
    public class InverseBoolToVisibilityConverterTests
    {
        [SetUp]
        public void SetUp()
        {
            _sut = new InverseBoolToVisibilityConverter();
        }

        private InverseBoolToVisibilityConverter _sut;

        [Test]
        public void ConvertBack_is_unsupported()
        {
            Assert.Throws<NotImplementedException>(() =>
                _sut.ConvertBack(null, null, null, CultureInfo.CurrentCulture));
        }

        [Test]
        public void False_gives_visible()
        {
            var result = _sut.Convert(false, typeof(Visibility), null, CultureInfo.CurrentCulture);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [Test]
        public void True_gives_hidden()
        {
            var result = _sut.Convert(true, typeof(Visibility), null, CultureInfo.CurrentCulture);

            Assert.AreEqual(Visibility.Hidden, result);
        }
    }
}