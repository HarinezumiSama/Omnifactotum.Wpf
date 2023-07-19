using System;
using NUnit.Framework;
using Omnifactotum.Wpf.Converters;

namespace Omnifactotum.Wpf.Tests
{
    [TestFixture(-1, 42, 1023, TypeArgs = new[] { typeof(int) })]
    [TestFixture("unfair", "fair", "maybe", TypeArgs = new[] { typeof(string) })]
    public sealed class BooleanToValueConverterTests<T>
    {
        #region Constants and Fields

        private readonly T _falseValue;
        private readonly T _trueValue;
        private readonly T _otherValue;
        private BooleanToValueConverter<T> _converter;

        #endregion

        #region Constructors

        public BooleanToValueConverterTests(T falseValue, T trueValue, T otherValue)
        {
            Assert.That(new[] { falseValue, trueValue, otherValue }, Is.Unique);

            _falseValue = falseValue;
            _trueValue = trueValue;
            _otherValue = otherValue;
        }

        #endregion

        #region SetUp/TearDown

        [SetUp]
        public void SetUp()
        {
            _converter = new BooleanToValueConverter<T>
            {
                FalseValue = _falseValue,
                TrueValue = _trueValue
            };
        }

        [TearDown]
        public void TearDown()
        {
            _converter = null;
        }

        #endregion

        #region Tests

        [Test]
        public void TestConvertWithInvalidArgumentThrows()
        {
            Assert.That(
                () => _converter.Convert(new Action(() => { }), typeof(T), null, null),
                Throws.ArgumentException.With.Property(nameof(ArgumentException.ParamName)).EqualTo("value"));

            Assert.That(
                () => _converter.Convert(false, typeof(Action), null, null),
                Throws.ArgumentException.With.Property(nameof(ArgumentException.ParamName)).EqualTo("targetType"));
        }

        [Test]
        public void TestConvertWithValidArgumentsSucceeds()
        {
            Assert.That(() => _converter.Convert(false, typeof(T), null, null), Is.EqualTo(_falseValue));
            Assert.That(() => _converter.Convert(true, typeof(T), null, null), Is.EqualTo(_trueValue));

            if (typeof(T).IsValueType && !typeof(T).IsNullableValueType())
            {
                Assert.That(
                    () => _converter.Convert(false, typeof(Nullable<>).MakeGenericType(typeof(T)), null, null),
                    Is.EqualTo(_falseValue));

                Assert.That(
                    () => _converter.Convert(true, typeof(Nullable<>).MakeGenericType(typeof(T)), null, null),
                    Is.EqualTo(_trueValue));
            }
        }

        [Test]
        public void TestConvertBackWithInvalidArgumentThrows()
        {
            Assert.That(
                () => _converter.ConvertBack(new Action(() => { }), typeof(bool), null, null),
                Throws.ArgumentException.With.Property(nameof(ArgumentException.ParamName)).EqualTo("value"));

            Assert.That(
                () => _converter.ConvertBack(_falseValue, typeof(Action), null, null),
                Throws.ArgumentException.With.Property(nameof(ArgumentException.ParamName)).EqualTo("targetType"));
        }

        [Test]
        public void TestConvertBackWithValidArgumentsSucceeds()
        {
            Assert.That(() => _converter.ConvertBack(_falseValue, typeof(bool), null, null), Is.False);
            Assert.That(() => _converter.ConvertBack(_falseValue, typeof(bool?), null, null), Is.False);

            Assert.That(() => _converter.ConvertBack(_trueValue, typeof(bool), null, null), Is.True);
            Assert.That(() => _converter.ConvertBack(_trueValue, typeof(bool?), null, null), Is.True);

            Assert.That(() => _converter.ConvertBack(_otherValue, typeof(bool), null, null), Is.False);
            Assert.That(() => _converter.ConvertBack(_otherValue, typeof(bool?), null, null), Is.False);
        }

        #endregion
    }
}