using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Omnifactotum.Wpf.Converters
{
    /// <summary>
    ///     Converts between two types using the assigned conversion handlers.
    /// </summary>
    public sealed class RelayValueConverter : IValueConverter
    {
        #region Delegates

        /// <summary>
        ///     Encapsulated the conversion method.
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
        ///     A converted value.
        /// </returns>
        public delegate object ConversionHandler(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when a value produced by the binding source needs to be converted to the type
        ///     of the binding target property.
        /// </summary>
        public event ConversionHandler ConvertForward;

        /// <summary>
        ///     Occurs when a value produced by the binding target needs to be converted to the type
        ///     of the binding source property.
        /// </summary>
        public event ConversionHandler ConvertBackward;

        #endregion

        #region IValueConverter Members

        /// <summary>
        ///     Converts a value.
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
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return RaiseConvertForward(value, targetType, parameter, culture);
        }

        /// <summary>
        ///     Converts a value.
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
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return RaiseConvertBackward(value, targetType, parameter, culture);
        }

        #endregion

        #region Private Methods

        private object RaiseConvertForward(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var handler = ConvertForward;
            if (handler == null)
            {
                if (WpfFactotum.IsInDesignMode())
                {
                    return null;
                }

                throw new NotSupportedException("The forward conversion event is not being handled.");
            }

            return handler(value, targetType, parameter, culture);
        }

        private object RaiseConvertBackward(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var handler = ConvertBackward;
            if (handler == null)
            {
                if (WpfFactotum.IsInDesignMode())
                {
                    return null;
                }

                throw new NotSupportedException("The backward conversion event is not being handled.");
            }

            return handler(value, targetType, parameter, culture);
        }

        #endregion
    }
}