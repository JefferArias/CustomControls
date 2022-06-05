using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomControls.Controls
{
    public partial class Frm
    {
        internal static class WIN
        {
            #region WINAPI

            [DllImport("dwmapi.dll")]
            public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

            [DllImport("dwmapi.dll")]
            public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

            public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
            {
                if (IntPtr.Size == 8)
                    return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
                else
                    return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
            }

            public static IntPtr SetClassLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
            {
                if (IntPtr.Size > 4)
                    return SetClassLongPtr64(hWnd, nIndex, dwNewLong);
                else
                    return new IntPtr(SetClassLongPtr32(hWnd, nIndex, unchecked((uint)dwNewLong.ToInt32())));
            }

            [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
            private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

            [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
            private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            [DllImport("user32.dll", EntryPoint = "SetClassLong")]
            private static extern uint SetClassLongPtr32(IntPtr hWnd, int nIndex, uint dwNewLong);

            [DllImport("user32.dll", EntryPoint = "SetClassLongPtr")]
            private static extern IntPtr SetClassLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            #endregion WINAPI

            #region Estructuras

            [StructLayout(LayoutKind.Sequential)]
            public struct MARGINS
            {
                public int leftWidth;
                public int rightWidth;
                public int topHeight;
                public int bottomHeight;
            }

            #endregion Estructuras

            #region Enumeraciones

            public enum WM : int
            {
                PAINT = 0xf,
                ACTIVATEAPP = 0x1c,
                NCPAINT = 0x85,
                SYSCOMMAND = 0x112,
                NCHITTEST = 0x84
            }

            public enum WS : int
            {
                WS_MAXIMIZEBOX = (int)0x10000L,
                WS_MINIMIZEBOX = (int)0x20000L
            }

            #endregion Enumeraciones
        }
    }
}
