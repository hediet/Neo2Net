using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ConsoleApplicationNeoTest
{
    class KeyBoardSimulator
    {

        /// <summary>
        /// Determines if the <see cref="VirtualKeyCode"/> is an ExtendedKey
        /// </summary>
        /// <param name="keyCode">The key code.</param>
        /// <returns>true if the key code is an extended key; otherwise, false.</returns>
        /// <remarks>
        /// The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; 
        /// the INS, DEL, HOME, END, PAGE UP, PAGE DOWN, and arrow keys in the clusters to the left of the numeric keypad; 
        /// the NUM LOCK key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in the numeric keypad.
        /// 
        /// See http://msdn.microsoft.com/en-us/library/ms646267(v=vs.85).aspx Section "Extended-Key Flag"
        /// </remarks>
        private static bool IsExtendedKey(Keys keyCode)
        {
            var code = (VirtualKeyCode)((int)keyCode);

            if (code == VirtualKeyCode.MENU ||
                code == VirtualKeyCode.LMENU ||
                code == VirtualKeyCode.RMENU ||
                code == VirtualKeyCode.CONTROL ||
                code == VirtualKeyCode.RCONTROL ||
                code == VirtualKeyCode.INSERT ||
                code == VirtualKeyCode.DELETE ||
                code == VirtualKeyCode.HOME ||
                code == VirtualKeyCode.END ||
                code == VirtualKeyCode.PRIOR ||
                code == VirtualKeyCode.NEXT ||
                code == VirtualKeyCode.RIGHT ||
                code == VirtualKeyCode.UP ||
                code == VirtualKeyCode.LEFT ||
                code == VirtualKeyCode.DOWN ||
                code == VirtualKeyCode.NUMLOCK ||
                code == VirtualKeyCode.CANCEL ||
                code == VirtualKeyCode.SNAPSHOT ||
                code == VirtualKeyCode.DIVIDE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private static void Send(ushort keyCode, ushort scanCode, uint flags)
        {
            var inputEntry = new Input
            {
                Type = (UInt32)InputType.Keyboard,
                Data =
                {
                    Keyboard =
                        new Keybdinput
                        {
                            KeyCode = keyCode,
                            Scan = scanCode,
                            Flags = flags,
                            Time = 0,
                            ExtraInfo = IntPtr.Zero
                        }
                }
            };
            
            var input = new[] { inputEntry };

            uint cSuccess = NativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(typeof(Input)));

            if (cSuccess != input.Length)
            {
                throw new Win32Exception();
            }
        }

        public static void Send(char character, KeyPressDirection pressDirection)
        {
            UInt16 scanCode = character;

            var flags = KeyboardFlag.Unicode;

            if (pressDirection == KeyPressDirection.Up)
                flags |= KeyboardFlag.KeyUp;

            // Handle extended keys:
            // If the scan code is preceded by a prefix byte that has the value 0xE0 (224),
            // we need to include the KEYEVENTF_EXTENDEDKEY flag in the Flags property. 
            if ((scanCode & 0xFF00) == 0xE000)
                flags |= KeyboardFlag.ExtendedKey;

            Send(0, scanCode, (UInt32)flags);
        }

        public static void Send(Keys keyCode, KeyPressDirection pressDirection)
        {
            var flags = (KeyboardFlag)0;

            if (pressDirection == KeyPressDirection.Up)
                flags |= KeyboardFlag.KeyUp;

            Send((UInt16)keyCode, (ushort)KeysHelper.ConvertToScanCode(keyCode), (UInt32)flags);
        }
    }
}