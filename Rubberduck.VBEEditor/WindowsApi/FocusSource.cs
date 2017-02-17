﻿using System;
using Rubberduck.Common.WinAPI;
using Rubberduck.VBEditor.Events;

namespace Rubberduck.VBEditor.WindowsApi
{
    internal abstract class FocusSource : SubclassingWindow, IWindowEventProvider
    {
        protected FocusSource(IntPtr hwnd) : base(hwnd, hwnd) { }

        public event EventHandler<WindowChangedEventArgs> FocusChange;
        protected void OnFocusChange(WindowChangedEventArgs eventArgs)
        {
            if (FocusChange != null)
            {
                FocusChange.Invoke(this, eventArgs);
            }
        }

        protected virtual void DispatchFocusEvent(WindowChangedEventArgs.FocusType type)
        {
            var window = VBEEvents.GetWindowInfoFromHwnd(Hwnd);
            if (window == null)
            {
                return;
            }
            OnFocusChange(new WindowChangedEventArgs(Hwnd, window.Value.Window, null, type));
        }

        public override int SubClassProc(IntPtr hWnd, IntPtr msg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, IntPtr dwRefData)
        {
            switch ((uint)msg)
            {
                case (uint)WM.SETFOCUS:

                    DispatchFocusEvent(WindowChangedEventArgs.FocusType.GotFocus);
                    break;
                case (uint)WM.KILLFOCUS:
                    DispatchFocusEvent(WindowChangedEventArgs.FocusType.LostFocus);
                    break;
            }
            return base.SubClassProc(hWnd, msg, wParam, lParam, uIdSubclass, dwRefData);
        }
    }
}
