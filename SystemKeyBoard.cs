using System;

namespace ConsoleApplicationNeoTest
{
    class SystemKeyBoard : IKeyboard
    {
        private static int counter;
        public static bool IsSending { get { return counter != 0; } }


        public void KeyEvent(Key key, KeyPressDirection pressDirection)
        {
            counter++;
            Console.WriteLine(pressDirection + " " + key);
            switch (key.KeyType)
            {
                case KeyType.Character:
                    KeyBoardSimulator.Send(key.Character, pressDirection);
                    break;
                case KeyType.KeyCode:
                    KeyBoardSimulator.Send(key.KeyCode, pressDirection);
                    break;
                default:
                    throw new Exception();
            }
            counter--;
        }
    }
}