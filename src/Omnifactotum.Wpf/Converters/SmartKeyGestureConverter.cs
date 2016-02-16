using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Omnifactotum.Wpf.Converters
{
    /// <summary>
    ///     The converter of <see cref="KeyGesture"/> which is capable of extracting related key
    ///     gestures and converting them into a single string.
    /// </summary>
    public sealed class SmartKeyGestureConverter : IValueConverter
    {
        #region Constants and Fields

        /// <summary>
        ///     The default separator used to separate multiple key gestures.
        /// </summary>
        public const string DefaultSeparator = " | ";

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SmartKeyGestureConverter"/> class.
        /// </summary>
        public SmartKeyGestureConverter()
        {
            Separator = DefaultSeparator;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the separator used to separate multiple key gestures. If set to
        ///     <c>null</c>, <see cref="DefaultSeparator"/> is used.
        /// </summary>
        public string Separator
        {
            get;
            set;
        }

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
        ///     A converted value. If the method returns <c>null</c>, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                return value;
            }

            var routedCommand = value as RoutedCommand;
            if (routedCommand != null)
            {
                return GetStringRepresentation(routedCommand.InputGestures);
            }

            var menuItem = value as MenuItem;
            if (menuItem != null)
            {
                var command = menuItem.Command;
                if (command == null)
                {
                    return string.Empty;
                }

                routedCommand = command as RoutedCommand;
                if (routedCommand != null)
                {
                    return GetStringRepresentation(routedCommand.InputGestures);
                }

                var parentWindow = menuItem.FindParent<Window>();
                if (parentWindow == null)
                {
                    return string.Empty;
                }

                var inputBindings = parentWindow
                    .InputBindings
                    .OfType<InputBinding>()
                    .Where(obj => obj.Command == command)
                    .Select(obj => obj.Gesture)
                    .OfType<KeyGesture>()
                    .ToArray();

                return GetKeyGestureStringRepresentation(inputBindings);
            }

            var collection = value as IEnumerable;
            return collection == null ? string.Empty : GetStringRepresentation(collection);
        }

        /// <summary>
        ///     Converts a value. This method is not supported in <see cref="SmartKeyGestureConverter"/>.
        /// </summary>
        /// <param name="value">
        ///     The value that is produced by the binding target.
        /// </param>
        /// <param name="targetType">
        ///     The type to convert to.
        /// </param>
        /// <param name="parameter">
        ///     The converter parameter to use.
        /// </param>
        /// <param name="culture">
        ///     The culture to use in the converter.
        /// </param>
        /// <returns>
        ///     In <see cref="SmartKeyGestureConverter"/>, this method always throws <see cref="NotSupportedException"/>.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Private Methods

        private string GetActualSeparator()
        {
            return Separator ?? DefaultSeparator;
        }

        private object GetKeyGestureStringRepresentation(IEnumerable<KeyGesture> keyGestures)
        {
            var actualSeparator = GetActualSeparator();

            var result = keyGestures
                .Select(item => item.GetDisplayStringForCulture(CultureInfo.InvariantCulture))
                .OrderBy(item => item.Length)
                .ThenBy(Factotum.Identity)
                .Join(actualSeparator);

            return result;
        }

        private object GetStringRepresentation(IEnumerable collection)
        {
            var keyGestures = collection.OfType<KeyGesture>();
            return GetKeyGestureStringRepresentation(keyGestures);
        }

        #endregion
    }
}