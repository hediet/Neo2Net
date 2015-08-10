using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    public static class KeysHelper
    {
        /// <summary>
        /// The set of valid MapTypes used in MapVirtualKey
        /// </summary>
        private enum MapVirtualKeyMapTypes : uint
        {
            /// <summary>
            /// uCode is a virtual-key code and is translated into a scan code.
            /// If it is a virtual-key code that does not distinguish between left- and
            /// right-hand keys, the left-hand scan code is returned.
            /// If there is no translation, the function returns 0.
            /// </summary>
            MAPVK_VK_TO_VSC = 0x00,

            /// <summary>
            /// uCode is a scan code and is translated into a virtual-key code that
            /// does not distinguish between left- and right-hand keys. If there is no
            /// translation, the function returns 0.
            /// </summary>
            MAPVK_VSC_TO_VK = 0x01,

            /// <summary>
            /// uCode is a virtual-key code and is translated into an unshifted
            /// character value in the low-order word of the return value. Dead keys (diacritics)
            /// are indicated by setting the top bit of the return value. If there is no
            /// translation, the function returns 0.
            /// </summary>
            MAPVK_VK_TO_CHAR = 0x02,

            /// <summary>
            /// Windows NT/2000/XP: uCode is a scan code and is translated into a
            /// virtual-key code that distinguishes between left- and right-hand keys. If
            /// there is no translation, the function returns 0.
            /// </summary>
            MAPVK_VSC_TO_VK_EX = 0x03,

            /// <summary>
            /// Not currently documented
            /// </summary>
            MAPVK_VK_TO_VSC_EX = 0x04
        }


        [DllImport("user32.dll")]
        private static extern uint MapVirtualKeyW(uint uCode, MapVirtualKeyMapTypes uMapType);

        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKeyCode, uint scanCode,
            byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
            StringBuilder receivingBuffer,
            int bufferSize, uint flags);


        static string GetCharsFromKeys(Keys key, bool shift, bool altGr)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];
            if (shift)
                keyboardState[(int)Keys.ShiftKey] = 0xff;
            if (altGr)
            {
                keyboardState[(int)Keys.ControlKey] = 0xff;
                keyboardState[(int)Keys.Menu] = 0xff;
            }
            ToUnicode((uint)key, 0, keyboardState, buf, 256, 0);
            return buf.ToString();
        }

        private static Dictionary<uint, Keys> keys2;

        public static Keys ConvertFromScanCode(uint scanCode)
        {
            return (Keys)MapVirtualKeyW(scanCode, MapVirtualKeyMapTypes.MAPVK_VSC_TO_VK);

            if (keys2 == null)
            {
                keys2 = new Dictionary<uint, Keys>();
                foreach (var k in Enum.GetValues(typeof(Keys)).OfType<Keys>())
                {
                    var scanCode2 = ConvertToScanCode(k);
                    if (!keys2.ContainsKey(scanCode2))
                        keys2[scanCode2] = k;
                    else
                        Debugger.Break();
                }
            }

            return keys2[scanCode];

            
        }

        public static uint ConvertToScanCode(Keys keys)
        {
            return MapVirtualKeyW((uint)keys, MapVirtualKeyMapTypes.MAPVK_VK_TO_VSC);
        }

        private static Dictionary<char, Keys> keys;


        public static Keys Convert(char character)
        {
            if (keys == null)
            {
                keys = new Dictionary<char, Keys>();
                foreach (var k in Enum.GetValues(typeof (Keys)).OfType<Keys>())
                {
                    var c = GetCharsFromKeys(k, false, false);
                    if (c.Length == 1)
                        keys[c[0]] = k;

                    c = GetCharsFromKeys(k, true, false);
                    if (c.Length == 1)
                        keys[c[0]] = k | Keys.Shift;

                    c = GetCharsFromKeys(k, false, true);
                    if (c.Length == 1)
                        keys[c[0]] = k | Keys.Alt;
                }
            }

            Keys result;
            if (keys.TryGetValue(character, out result))
                return result;

            return Keys.None;
        }
    }
}