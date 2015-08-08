using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Open.WinKeyboardHook;


namespace ConsoleApplicationNeoTest
{
    class DumpKeyboard : IKeyboard
    {
        public void KeyEvent(Key key, KeyPressDirection pressDirection)
        {
            Console.WriteLine(pressDirection + " " + key);

        }
    }


    class Program
    {

        static void Main(string[] args)
        {
            if (!Debugger.IsAttached)
            {
                var kb2 = new DumpKeyboard();

                using (new SystemKeyBoardInterceptor(kb2, false))
                {
                    Application.Run();
                }
                return;
            }
            
            var kb = new NeoKeyboard(new SystemKeyBoard());

            using (new SystemKeyBoardInterceptor(kb, true))
            {
                Application.Run();
            }
        }
    }
}
