using System;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    class SendInputKeyboard : IKeyboard
    {
        private static int counter;
        public static bool IsSending { get { return counter != 0; } }


        public void KeyEvent(Key key, KeyPressDirection pressDirection)
        {
            counter++;
            if (key.KeyCode == Keys.Packet)
                throw new Exception("Cannot send packet.");
            switch (key.KeyType)
            {
                case KeyType.Character:
                    SendInput.Send(key.Character, pressDirection);
                    break;
                case KeyType.KeyCode:
                    SendInput.Send(key.KeyCode, pressDirection);
                    break;
                default:
                    throw new Exception();
            }
            counter--;
        }
    }
}