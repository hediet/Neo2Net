using System.Runtime.InteropServices;

namespace ConsoleApplicationNeoTest
{
    /// <summary>
    /// The combined/overlayed structure that includes Mouse, Keyboard and Hardware Input message data (see: http://msdn.microsoft.com/en-us/library/ms646270(VS.85).aspx)
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct MOUSEKEYBDHARDWAREINPUT
    {
        /// <summary>
        /// The <see cref="MOUSEINPUT"/> definition.
        /// </summary>
        [FieldOffset(0)]
        public Mouseinput Mouse;

        /// <summary>
        /// The <see cref="KEYBDINPUT"/> definition.
        /// </summary>
        [FieldOffset(0)]
        public Keybdinput Keyboard;

        /// <summary>
        /// The <see cref="HARDWAREINPUT"/> definition.
        /// </summary>
        [FieldOffset(0)]
        public Hardwareinput Hardware;
    }
}