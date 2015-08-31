using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Omnifactotum.Wpf.Converters
{
    /// <summary>
    ///     Converts between nullable <see cref="bool"/> and <see cref="Visibility"/>.
    /// </summary>
    public sealed class NullableBooleanToVisibilityConverter : NullableBooleanToValueConverter<Visibility>
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NullableBooleanToVisibilityConverter"/> class.
        /// </summary>
        public NullableBooleanToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
            NullValue = Visibility.Collapsed;
        }

        #endregion
    }
}