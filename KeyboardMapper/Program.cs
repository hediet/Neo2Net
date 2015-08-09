using System;
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


    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "show")
            {
                Console.WriteLine("Show");
                var kb2 = new DumpKeyboard();

                using (new WindowsKeyBoardInterceptor(kb2, false))
                {
                    Application.Run();
                }
                return;
            }


            var kb = new Keyboard(new SendInputKeyboard());

            using (new WindowsKeyBoardInterceptor(kb))
            {
                Application.Run();
            }
        }
    }
}
