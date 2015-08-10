using System;
using System.Drawing;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper.Mouse
{
    public class WindowsMouse: IMouse
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        [Flags]
        private enum MouseEventTFlags
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


        public void SetButtonState(MouseButton button, KeyPressDirection direction)
        {
            var flag = MouseEventTFlags.LEFTDOWN;
            if (direction == KeyPressDirection.Down)
            {
                if (button == MouseButton.Left) 
                    flag = MouseEventTFlags.LEFTDOWN;
                else if (button == MouseButton.Middle)
                    flag = MouseEventTFlags.MIDDLEDOWN;
                else if (button == MouseButton.Right)
                    flag = MouseEventTFlags.RIGHTDOWN;
            }
            else if (direction == KeyPressDirection.Up)
            {
                if (button == MouseButton.Left)
                    flag = MouseEventTFlags.LEFTUP;
                else if (button == MouseButton.Middle)
                    flag = MouseEventTFlags.MIDDLEUP;
                else if (button == MouseButton.Right)
                    flag = MouseEventTFlags.RIGHTUP;
            }
            
            mouse_event((uint)flag, 0, 0, 0, UIntPtr.Zero);
        }

        public Point Position
        {
            get { return Cursor.Position; }
            set { Cursor.Position = value; }
        }
    }
}