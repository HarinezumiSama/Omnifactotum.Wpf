using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Omnifactotum.Wpf.Converters
{
    /// <summary>
    ///     Converts between two types using the assigned conversion handlers.
    /// </summary>
    public sealed class RelayValueConverter : IValueConverter
    {
        #region Constants and Fields

        internal const string ConvertForwardName = nameof(ConvertForward);
        internal const string ConvertBackwardName = nameof(ConvertBackward);

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when a value produced by the binding source needs to be converted to the type
        ///     of the binding target property.
        /// </summary>
        public event RelayConversionHandler ConvertForward;

        /// <summary>
        ///     Occurs when a value produced by the binding target needs to be converted to the type
        ///     of the binding source property.
        /// </summary>
        public event RelayConversionHandler ConvertBackward;

        #endregion

        #region IValueConverter Members

        /// <summary>
        ///     Converts a value produced by the binding source to the type of the binding target property.
        /// </summary>
        /// <param name="value">
        ///     The value produced by the binding source.
        /// </param>
        /// <param name="targetType">
        ///     The type of the binding target property.
        /// </param>
        /// <param name="parameter">
        ///     The converter parameter to use.
        /// </param>
        /// <param name="culture">
        ///     The culture to use in the converter.
        /// </param>
        /// <returns>
        ///     A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return RaiseConvertForward(value, targetType, parameter, culture);
        }

        /// <summary>
        ///     Converts a value produced by the binding target to the type of the binding source property.
        /// </summary>
        /// <param name="value">
        ///     The value that is produced by the binding target.
        /// </param>
        /// <param name="targetType">
        ///     The type of the binding source property.
        /// </param>
        /// <param name="parameter">
        ///     The converter parameter to use.
        /// </param>
        /// <param name="culture">
        ///     The culture to use in the converter.
        /// </param>
        /// <returns>
        ///     A converted value.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return RaiseConvertBackward(value, targetType, parameter, culture);
        }

        #endregion

        #region Private Methods

        private static object RaiseInternal(
            RelayConversionHandler handler,
            string eventName,
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (handler != null)
            {
                return handler(value, targetType, parameter, culture);
            }

            if (WpfFactotum.IsInDesignMode())
            {
                return DependencyProperty.UnsetValue;
            }

            throw new NotSupportedException(
                string.Format(CultureInfo.InvariantCulture, "The event '{0}' is not being handled.", eventName));
        }

        private object RaiseConvertForward(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return RaiseInternal(ConvertForward, nameof(ConvertForward), value, targetType, parameter, culture);
        }

        private object RaiseConvertBackward(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return RaiseInternal(ConvertBackward, nameof(ConvertBackward), value, targetType, parameter, culture);
        }

        #endregion
    }
}