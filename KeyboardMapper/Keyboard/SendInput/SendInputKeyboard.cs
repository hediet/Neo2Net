using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    class SendInputKeyboard : IKeyboard
    {
        private static readonly List<Key> sendingKeys = new List<Key>();

        public static Key[] SendingKeys
        {
            get
            {
                lock (sendingKeys)
                {
                    return sendingKeys.ToArray();
                }
            }
        }


        public void KeyEvent(Key key, KeyPressDirection pressDirection)
        {
            lock (sendingKeys)
            {
                sendingKeys.Add(key);
            }
            
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
            
            lock (sendingKeys)
            {
                sendingKeys.Remove(key);
            }
        }
    }
}