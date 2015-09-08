using System;
using System.Globalization;
using System.Linq;

namespace Omnifactotum.Wpf.Converters
{
    /// <summary>
    ///     Encapsulated the conversion method for <see cref="RelayValueConverter"/>.
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
    public delegate object RelayConversionHandler(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture);
}