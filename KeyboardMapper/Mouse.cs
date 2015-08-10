using System;
using System.Drawing;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    interface IMouse
    {
        Point Position { get; set; }
    }

    public class Mouse: IMouse
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        [Flags]
        public enum MouseEventTFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }


        public void DoubleClick()
        {
            mouse_event((uint)MouseEventTFlags.LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event((uint)MouseEventTFlags.LEFTUP, 0, 0, 0, UIntPtr.Zero);
            mouse_event((uint)MouseEventTFlags.LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event((uint)MouseEventTFlags.LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

        public void LeftClick()
        {
            mouse_event((uint)MouseEventTFlags.LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event((uint)MouseEventTFlags.LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

        public void LeftUp()
        {
            mouse_event((uint)MouseEventTFlags.LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

        public void LeftDown()
        {
            mouse_event((uint)MouseEventTFlags.LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
        }

        public void RightClick()
        {
            mouse_event((uint)MouseEventTFlags.RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event((uint)MouseEventTFlags.RIGHTUP, 0, 0, 0, UIntPtr.Zero);
        }

        public void RightUp()
        {
            mouse_event((uint)MouseEventTFlags.RIGHTUP, 0, 0, 0, UIntPtr.Zero);
        }

        public void RightDown()
        {
            mouse_event((uint)MouseEventTFlags.RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
        }

        public void MiddleClick()
        {
            mouse_event((uint)MouseEventTFlags.MIDDLEDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event((uint)MouseEventTFlags.MIDDLEUP, 0, 0, 0, UIntPtr.Zero);
        }

        public void MiddleUp()
        {
            mouse_event((uint)MouseEventTFlags.MIDDLEUP, 0, 0, 0, UIntPtr.Zero);
        }

        public void MiddleDown()
        {
            mouse_event((uint)MouseEventTFlags.MIDDLEDOWN, 0, 0, 0, UIntPtr.Zero);
        }

        public void SetPositionOfCursor(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public Point Position
        {
            get { return Cursor.Position; }
            set { Cursor.Position = value; }
        }
    }
}