using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Omnifactotum.Annotations;

namespace Omnifactotum.Wpf
{
    /// <summary>
    /// Contains the attached properties allowing to specify extra styles for <see cref="Window" />.
    /// </summary>
    public static class WindowStyles
    {
        #region Constants and Fields

        /// <summary>
        ///     The identifier for the <see cref="P:WindowStyles.CanMinimize"/> attached property.
        /// </summary>
        public static readonly DependencyProperty CanMinimizeProperty =
            DependencyProperty.RegisterAttached(
                "CanMinimize",
                typeof(bool),
                typeof(WindowStyles),
                new UIPropertyMetadata(true, OnCanMinimizeChanged));

        /// <summary>
        ///     The identifier for the <see cref="P:WindowStyles.CanMaximize"/> attached property.
        /// </summary>
        public static readonly DependencyProperty CanMaximizeProperty =
            DependencyProperty.RegisterAttached(
                "CanMaximize",
                typeof(bool),
                typeof(WindowStyles),
                new UIPropertyMetadata(true, OnCanMaximizeChanged));

        /// <summary>
        ///     The identifier for the <see cref="P:WindowStyles.HasSystemMenu"/> attached property.
        /// </summary>
        public static readonly DependencyProperty HasSystemMenuProperty =
            DependencyProperty.RegisterAttached(
                "HasSystemMenu",
                typeof(bool),
                typeof(WindowStyles),
                new UIPropertyMetadata(true, OnHasSystemMenuChanged));

        //// ReSharper disable InconsistentNaming - WinAPI imports
        private const int SWP_FRAMECHANGED = 0x0020;

        private const int SWP_NOACTIVATE = 0x0010;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOOWNERZORDER = 0x0200;
        private const int SWP_NOREPOSITION = 0x0200;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int GWL_STYLE = -16;
        //// ReSharper restore InconsistentNaming - WinAPI imports

        private const string User32Dll = "user32.dll";

        #endregion

        #region ApiWindowStyles Enumeration

        [Flags]
        private enum ApiWindowStyles : uint
        {
            //// ReSharper disable InconsistentNaming - WinAPI imports
            WS_SYSMENU = 0x80000,

            WS_MINIMIZEBOX = 0x20000,
            WS_MAXIMIZEBOX = 0x10000,
            //// ReSharper restore InconsistentNaming - WinAPI imports
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Gets the value of the <see cref="P:WindowStyles.CanMinimize"/> attached property
        ///     from the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">
        ///     The window from which to read the property value.
        /// </param>
        /// <returns>
        ///     The value of the <see cref="P:WindowStyles.CanMinimize"/> attached property.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="window"/> is <c>null</c>.
        /// </exception>
        public static bool GetCanMinimize(Window window)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            return (bool)window.GetValue(CanMinimizeProperty);
        }

        /// <summary>
        ///     Sets the value of the <see cref="P:WindowStyles.CanMinimize"/> attached property to
        ///     the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">
        ///     The window on which to set the <see cref="P:WindowStyles.CanMinimize"/> attached property.
        /// </param>
        /// <param name="value">
        ///     The property value to set.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="window"/> is <c>null</c>.
        /// </exception>
        public static void SetCanMinimize(Window window, bool value)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            window.SetValue(CanMinimizeProperty, value);
        }

        /// <summary>
        ///     Gets the value of the <see cref="P:WindowStyles.CanMaximize"/> attached property
        ///     from the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">
        ///     The window from which to read the property value.
        /// </param>
        /// <returns>
        ///     The value of the <see cref="P:WindowStyles.CanMaximize"/> attached property.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="window"/> is <c>null</c>.
        /// </exception>
        public static bool GetCanMaximize(Window window)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            return (bool)window.GetValue(CanMaximizeProperty);
        }

        /// <summary>
        ///     Sets the value of the <see cref="P:WindowStyles.CanMaximize"/> attached property to
        ///     the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">
        ///     The window on which to set the <see cref="P:WindowStyles.CanMaximize"/> attached property.
        /// </param>
        /// <param name="value">
        ///     The property value to set.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="window"/> is <c>null</c>.
        /// </exception>
        public static void SetCanMaximize(Window window, bool value)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            window.SetValue(CanMaximizeProperty, value);
        }

        /// <summary>
        ///     Gets the value of the <see cref="P:WindowStyles.HasSystemMenu"/> attached property
        ///     from the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">
        ///     The window from which to read the property value.
        /// </param>
        /// <returns>
        ///     The value of the <see cref="P:WindowStyles.HasSystemMenu"/> attached property.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="window"/> is <c>null</c>.
        /// </exception>
        public static bool GetHasSystemMenu(Window window)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            return (bool)window.GetValue(HasSystemMenuProperty);
        }

        /// <summary>
        ///     Sets the value of the <see cref="P:WindowStyles.HasSystemMenu"/> attached property to
        ///     the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">
        ///     The window on which to set the <see cref="P:WindowStyles.HasSystemMenu"/> attached property.
        /// </param>
        /// <param name="value">
        ///     The property value to set.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="window"/> is <c>null</c>.
        /// </exception>
        public static void SetHasSystemMenu(Window window, bool value)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            window.SetValue(HasSystemMenuProperty, value);
        }

        #endregion

        #region Private Methods: WinAPI Imports and Helpers

        [DllImport(User32Dll, EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport(User32Dll, EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLong64(IntPtr hWnd, int nIndex);

        [DllImport(User32Dll, EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport(User32Dll, EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLong64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport(User32Dll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int x,
            int y,
            int cx,
            int cy,
            uint uFlags);

        private static bool Is64Bit()
        {
            return IntPtr.Size == 8;
        }

        private static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
        {
            return Is64Bit() ? GetWindowLong64(hWnd, nIndex) : GetWindowLong32(hWnd, nIndex);
        }

        private static void SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (Is64Bit())
            {
                SetWindowLong64(hWnd, nIndex, dwNewLong);
            }
            else
            {
                SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32());
            }
        }

        private static void ResetWindowStyle([NotNull] Window window, ApiWindowStyles styles, bool set)
        {
            const int Flags = SWP_FRAMECHANGED | SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOOWNERZORDER | SWP_NOREPOSITION
                | SWP_NOSIZE | SWP_NOZORDER;

            var wih = new WindowInteropHelper(window);
            var style = (ApiWindowStyles)GetWindowLong(wih.EnsureHandle(), GWL_STYLE);

            if (set)
            {
                style |= styles;
            }
            else
            {
                style &= ~styles;
            }

            SetWindowLong(wih.Handle, GWL_STYLE, (IntPtr)style);
            SetWindowPos(wih.Handle, IntPtr.Zero, 0, 0, 0, 0, Flags);
        }

        private static void AffectWindowStyle(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs args,
            ApiWindowStyles styles)
        {
            var window = obj as Window;
            if (window == null)
            {
                return;
            }

            ResetWindowStyle(window, styles, (bool)args.NewValue);
        }

        #endregion

        #region Private Methods: Regular

        private static void OnCanMaximizeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            AffectWindowStyle(obj, args, ApiWindowStyles.WS_MAXIMIZEBOX);
        }

        private static void OnCanMinimizeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            AffectWindowStyle(obj, args, ApiWindowStyles.WS_MINIMIZEBOX);
        }

        private static void OnHasSystemMenuChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            AffectWindowStyle(obj, args, ApiWindowStyles.WS_SYSMENU);
        }

        #endregion
    }
}