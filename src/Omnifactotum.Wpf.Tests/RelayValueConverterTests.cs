using System;
using NUnit.Framework;
using Omnifactotum.Wpf.Converters;

namespace Omnifactotum.Wpf.Tests
{
    [TestFixture]
    public sealed class RelayValueConverterTests
    {
        #region Tests

        [Test]
        public void TestConvertWithUnassignedConvertForwardEventThrows()
        {
            var converter = new RelayValueConverter();
            converter.ConvertBackward += (value, type, parameter, culture) => null;

            Assert.That(
                () => converter.Convert(1, typeof(int), null, null),
                Throws.TypeOf<NotSupportedException>().With.Message.Contains(RelayValueConverter.ConvertForwardName));
        }

        [Test]
        public void TestConvertWithAssignedConvertForwardEventSucceeds()
        {
            var converter = new RelayValueConverter();
            converter.ConvertForward += (value, type, parameter, culture) => value.ToString();

            Assert.That(
                () => converter.Convert(42, null, null, null),
                Is.EqualTo("42"));
        }

        [Test]
        public void TestConvertBackWithUnassignedConvertBackwardEventThrows()
        {
            var converter = new RelayValueConverter();
            converter.ConvertForward += (value, type, parameter, culture) => null;

            Assert.That(
                () => converter.ConvertBack(1, typeof(int), null, null),
                Throws.TypeOf<NotSupportedException>().With.Message.Contains(RelayValueConverter.ConvertBackwardName));
        }

        [Test]
        public void TestConvertBackWithAssignedConvertForwardEventSucceeds()
        {
            var converter = new RelayValueConverter();
            converter.ConvertBackward += (value, type, parameter, culture) => int.Parse((string)value);

            Assert.That(
                () => converter.ConvertBack("42", null, null, null),
                Is.EqualTo(42));
        }

        #endregion
    }
}