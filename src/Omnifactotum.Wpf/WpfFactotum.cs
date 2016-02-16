using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Omnifactotum.Annotations;

namespace Omnifactotum.Wpf
{
    /// <summary>
    ///     Provides helper methods for use in WPF projects.
    /// </summary>
    public static partial class WpfFactotum
    {
        #region Public Methods

        /// <summary>
        ///     Determines whether the code is running in the context of a designer.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the code is running in the context of a designer; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInDesignMode()
        {
            try
            {
                return (bool)DesignerProperties
                    .IsInDesignModeProperty
                    .GetMetadata(typeof(DependencyObject))
                    .DefaultValue;
            }
            catch (Exception ex)
            {
                if (ex.IsFatal())
                {
                    throw;
                }

                return false;
            }
        }

        /// <summary>
        ///     Finds the parent of the specified type for the specified <see cref="DependencyObject"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the parent to find.
        /// </typeparam>
        /// <param name="dependencyObject">
        ///     The dependency object to find the parent of the specified type for.
        /// </param>
        /// <returns>
        ///     A parent of the specified type <typeparamref name="T"/>, or <c>null</c> if no
        ///     parent of the specified type was found.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="dependencyObject"/> is neither <see cref="Visual"/> nor <see cref="Visual3D"/>.
        /// </exception>
        [CanBeNull]
        public static T FindParent<T>([CanBeNull] this DependencyObject dependencyObject)
            where T : DependencyObject
        {
            var processedObjects = new HashSet<DependencyObject>(
                ByReferenceEqualityComparer<DependencyObject>.Instance);

            var visualParent = FindVisualParent<T>(dependencyObject, processedObjects);
            if (visualParent != null)
            {
                return visualParent;
            }

            var current = dependencyObject;
            while (current != null)
            {
                current = (current as FrameworkElement)?.Parent;

                var result = current as T;
                if (result != null)
                {
                    return result;
                }

                result = FindVisualParent<T>(current, processedObjects);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        #endregion

        #region Private Methods

        internal static T FindVisualParent<T>(
            DependencyObject dependencyObject,
            HashSet<DependencyObject> processedObjects)
            where T : DependencyObject
        {
            var current = dependencyObject;
            while (current != null)
            {
                if (processedObjects.Contains(current))
                {
                    break;
                }

                processedObjects.Add(current);

                if (!(current is Visual) && !(current is Visual3D))
                {
                    break;
                }

                current = VisualTreeHelper.GetParent(current);

                var castObject = current as T;
                if (castObject != null)
                {
                    return castObject;
                }
            }

            return null;
        }

        #endregion
    }
}