using System;
using NUnit.Framework;
using Omnifactotum.Wpf.Converters;

namespace Omnifactotum.Wpf.Tests
{
    [TestFixture(-1, 42, -15, 1023, TypeArgs = new[] { typeof(int) })]
    [TestFixture("unfair", "fair", "unsure", "who knows", TypeArgs = new[] { typeof(string) })]
    public sealed class NullableBooleanToValueConverterTests<T>
    {
        #region Constants and Fields

        private readonly T _falseValue;
        private readonly T _trueValue;
        private readonly T _nullValue;
        private readonly T _otherValue;
        private NullableBooleanToValueConverter<T> _converter;

        #endregion

        #region Constructors

        public NullableBooleanToValueConverterTests(T falseValue, T trueValue, T nullValue, T otherValue)
        {
            Assert.That(new[] { falseValue, trueValue, nullValue, otherValue }, Is.Unique);

            _falseValue = falseValue;
            _trueValue = trueValue;
            _nullValue = nullValue;
            _otherValue = otherValue;
        }

        #endregion

        #region SetUp/TearDown

        [SetUp]
        public void SetUp()
        {
            _converter = new NullableBooleanToValueConverter<T>
            {
                FalseValue = _falseValue,
                TrueValue = _trueValue,
                NullValue = _nullValue
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
            Assert.That(() => _converter.Convert(null, typeof(T), null, null), Is.EqualTo(_nullValue));

            if (typeof(T).IsValueType && !typeof(T).IsNullable())
            {
                var genericType = typeof(Nullable<>).MakeGenericType(typeof(T));

                Assert.That(() => _converter.Convert(false, genericType, null, null), Is.EqualTo(_falseValue));
                Assert.That(() => _converter.Convert(true, genericType, null, null), Is.EqualTo(_trueValue));
                Assert.That(() => _converter.Convert(null, genericType, null, null), Is.EqualTo(_nullValue));
            }
        }

        [Test]
        public void TestConvertBackWithInvalidArgumentThrows()
        {
            Assert.That(
                () => _converter.ConvertBack(new Action(() => { }), typeof(bool?), null, null),
                Throws.ArgumentException.With.Property(nameof(ArgumentException.ParamName)).EqualTo("value"));

            Assert.That(
                () => _converter.ConvertBack(_falseValue, typeof(Action), null, null),
                Throws.ArgumentException.With.Property(nameof(ArgumentException.ParamName)).EqualTo("targetType"));
        }

        [Test]
        public void TestConvertBackWithValidArgumentsSucceeds()
        {
            Assert.That(() => _converter.ConvertBack(_falseValue, typeof(bool?), null, null), Is.False);
            Assert.That(() => _converter.ConvertBack(_trueValue, typeof(bool?), null, null), Is.True);
            Assert.That(() => _converter.ConvertBack(_nullValue, typeof(bool?), null, null), Is.Null);
            Assert.That(() => _converter.ConvertBack(_otherValue, typeof(bool?), null, null), Is.False);
        }

        #endregion
    }
}