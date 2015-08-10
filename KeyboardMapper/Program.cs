using System;
using System.Drawing;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    class DumpKeyboard : IKeyboard
    {
        public void KeyEvent(Key key, KeyPressDirection pressDirection)
        {
            Console.WriteLine(pressDirection + " " + key + " scancode: " + KeysHelper.ConvertToScanCode(key.KeyCode));

        }
    }

    class SemanticDumpKeyboard: ISemanticKeyboard
    {
        public void KeyEvent(SemanticKey key, KeyPressDirection pressDirection)
        {
            Console.WriteLine(pressDirection + " " + key);
        }
    }

    class Program
    {

        static void Main5(string[] args)
        {
            var kbd = new Keyboard(new DumpKeyboard(), new SemanticDumpKeyboard());
            kbd.KeyEvent(new Key((Keys)226), KeyPressDirection.Down);
            kbd.KeyEvent(new Key((Keys)226), KeyPressDirection.Down);
            
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            Console.BufferWidth = 200;

            if (args.Length == 1 && args[0] == "show")
            {
                Console.WriteLine("Show");

                using (new WindowsKeyboardInterceptor(new DumpKeyboard(), false))
                {
                    Application.Run();
                }
                return;
            }

            var icon = new TrayIcon();

            var sik = new SendInputKeyboard();

            using (new WindowsKeyboardInterceptor(new Keyboard(sik, new SemanticKeyboard(sik))))
            {
                Application.Run();
            }
        }
    }
}
