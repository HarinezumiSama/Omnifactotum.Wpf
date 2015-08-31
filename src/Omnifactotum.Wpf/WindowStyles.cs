using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Omnifactotum.Annotations;

namespace Omnifactotum.Wpf
{
    internal static class WindowStyles
    {
        #region Constants and Fields

        public static readonly DependencyProperty CanMinimizeProperty =
            DependencyProperty.RegisterAttached(
                "CanMinimize",
                typeof(bool),
                typeof(WindowStyles),
                new UIPropertyMetadata(true, OnCanMinimizeChanged));

        public static readonly DependencyProperty CanMaximizeProperty =
            DependencyProperty.RegisterAttached(
                "CanMaximize",
                typeof(bool),
                typeof(WindowStyles),
                new UIPropertyMetadata(true, OnCanMaximizeChanged));

        public static readonly DependencyProperty HasSystemMenuProperty =
            DependencyProperty.RegisterAttached(
                "HasSystemMenu",
                typeof(bool),
                typeof(WindowStyles),
                new UIPropertyMetadata(true, OnHasSystemMenuChanged));

        //// ReSharper disable once InconsistentNaming - WinAPI import
        private const int SWP_FRAMECHANGED = 0x0020;

        //// ReSharper disable once InconsistentNaming - WinAPI import
        private const int SWP_NOACTIVATE = 0x0010;

        //// ReSharper disable once InconsistentNaming - WinAPI import
        private const int SWP_NOMOVE = 0x0002;

        //// ReSharper disable once InconsistentNaming - WinAPI import
        private const int SWP_NOOWNERZORDER = 0x0200;

        //// ReSharper disable once InconsistentNaming - WinAPI import
        private const int SWP_NOREPOSITION = 0x0200;

        //// ReSharper disable once InconsistentNaming - WinAPI import
        private const int SWP_NOSIZE = 0x0001;

        //// ReSharper disable once InconsistentNaming - WinAPI import
        private const int SWP_NOZORDER = 0x0004;

        //// ReSharper disable once InconsistentNaming - WinAPI import
        private const int GWL_STYLE = -16;

        private const string User32Dll = "user32.dll";

        #endregion

        #region ApiWindowStyles Enumeration

        [Flags]
        private enum ApiWindowStyles : uint
        {
            //// ReSharper disable once InconsistentNaming - WinAPI import
            WS_SYSMENU = 0x80000,

            //// ReSharper disable once InconsistentNaming - WinAPI import
            WS_MINIMIZEBOX = 0x20000,

            //// ReSharper disable once InconsistentNaming - WinAPI import
            WS_MAXIMIZEBOX = 0x10000,
        }

        #endregion

        #region Public Methods

        public static bool GetCanMinimize(Window obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return (bool)obj.GetValue(CanMinimizeProperty);
        }

        public static void SetCanMinimize(Window obj, bool value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.SetValue(CanMinimizeProperty, value);
        }

        public static bool GetCanMaximize(Window obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return (bool)obj.GetValue(CanMaximizeProperty);
        }

        public static void SetCanMaximize(Window obj, bool value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.SetValue(CanMaximizeProperty, value);
        }

        public static bool GetHasSystemMenu(Window obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return (bool)obj.GetValue(HasSystemMenuProperty);
        }

        public static void SetHasSystemMenu(Window obj, bool value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.SetValue(HasSystemMenuProperty, value);
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