using System.Globalization;
using System.Windows;
using NUnit.Framework;
using Pomo_Shiny.Converters;

namespace Tests
{
    [TestFixture]
    public class BoolToVisibilityConverterTests
    {
        [Test]
        public void False_gives_hidden()
        {
            var sut = new BoolToVisibilityConverter();
            var result = sut.Convert(false, typeof(Visibility), null, CultureInfo.CurrentCulture);

            Assert.AreEqual(Visibility.Hidden, result);
        }

        [Test]
        public void True_gives_visible()
        {
            var sut = new BoolToVisibilityConverter();
            var result = sut.Convert(true, typeof(Visibility), null, CultureInfo.CurrentCulture);

            Assert.AreEqual(Visibility.Visible, result);
        }
    }
}