using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ConsoleApplicationNeoTest
{
    static class KeysHelper
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


        public static Keys Convert(uint scanCode)
        {
            return (Keys)MapVirtualKeyW(scanCode, MapVirtualKeyMapTypes.MAPVK_VSC_TO_VK);
        }

        public static uint ConvertToScanCode(Keys keys)
        {
            return MapVirtualKeyW((uint)keys, MapVirtualKeyMapTypes.MAPVK_VK_TO_VSC);
        }

        private static Dictionary<char, Keys> keys;

        public static Keys? Convert(char character)
        {
            character = char.ToUpper(character);
            if (keys == null)
            {
                keys = new Dictionary<char, Keys>();
                foreach (var k in Enum.GetValues(typeof (Keys)).OfType<Keys>())
                {
                    var c = (char) MapVirtualKeyW((uint)k, MapVirtualKeyMapTypes.MAPVK_VK_TO_CHAR);
                    if (c != 0)
                        keys[c] = k;
                }
            }

            Keys result;
            if (keys.TryGetValue(character, out result))
                return result;
            return null;
        }
    }
}