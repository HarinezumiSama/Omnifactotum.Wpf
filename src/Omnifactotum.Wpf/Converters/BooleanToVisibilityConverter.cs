using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Omnifactotum.Wpf.Converters
{
    /// <summary>
    ///     Converts between <see cref="bool"/> and <see cref="Visibility"/>.
    /// </summary>
    public sealed class BooleanToVisibilityConverter : BooleanToValueConverter<Visibility>
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BooleanToVisibilityConverter"/> class.
        /// </summary>
        public BooleanToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        #endregion
    }
}