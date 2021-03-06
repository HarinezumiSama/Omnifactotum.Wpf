﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Omnifactotum.Wpf.Converters
{
    /// <summary>
    ///     Converts between <see cref="bool"/> and the type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     The type to convert to and from <see cref="bool"/>.
    /// </typeparam>
    public class BooleanToValueConverter<T> : IValueConverter
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BooleanToValueConverter{T}"/> class.
        /// </summary>
        public BooleanToValueConverter()
        {
            TrueValue = default(T);
            FalseValue = default(T);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the value of the type <typeparamref name="T"/> which corresponds to <c>true</c>.
        /// </summary>
        public T TrueValue
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets the value of the type <typeparamref name="T"/> which corresponds to <c>false</c>.
        /// </summary>
        public T FalseValue
        {
            get;
            set;
        }

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
            if (!(value is bool))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        @"The value must be of the type '{0}'.",
                        typeof(bool).Name),
                    nameof(value));
            }

            if (!targetType.IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The type '{0}' is not compatible with '{1}'.",
                        targetType.GetQualifiedName(),
                        typeof(T).GetQualifiedName()),
                    nameof(targetType));
            }

            return (bool)value ? TrueValue : FalseValue;
        }

        /// <summary>
        ///     Converts a value produced by the binding target to the type of the binding source property.
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
        ///     A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            #region Argument Check

            if (!(value is T))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        @"The value must be of the type '{0}'.",
                        typeof(T).GetQualifiedName()),
                    nameof(value));
            }

            if (!targetType.IsAssignableFrom(typeof(bool)))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The type '{0}' is not compatible with '{1}'.",
                        targetType.GetQualifiedName(),
                        typeof(bool).GetQualifiedName()),
                    nameof(targetType));
            }

            #endregion

            return EqualityComparer<T>.Default.Equals((T)value, TrueValue);
        }

        #endregion
    }
}