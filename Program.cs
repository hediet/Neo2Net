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
using Tyml.ConsoleHelper;
using Tyml.Nodes;
using Tyml.Nodes.Immutable;
using Tyml.Serialization;


namespace ConsoleApplicationNeoTest
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
            /*
            Action<string> format = f =>
            {
                var s = new TymlSerializer();
                var o = s.Deserialize<object>(TymlParser.ParseFile(f).Root);
                File.WriteAllText(f, s.SerializeToDocument(o).Format().Text);
            };

            format(@"C:\Henning\Coding\Projects\Neo2Net\Data\KeyDefinitions.tyml");
            format(@"C:\Henning\Coding\Projects\Neo2Net\Data\KeyMappings.tyml");

            return;
            */
            //new ConfigGenerator.ConfigGenerator();
            

            if (args.Length == 1 && args[0] == "show")
            {
                Console.WriteLine("Show");
                var kb2 = new DumpKeyboard();

                using (new SystemKeyBoardInterceptor(kb2, false))
                {
                    Application.Run();
                }
                return;
            }


            var kb = new Keyboard(new SystemKeyBoard());

            using (new SystemKeyBoardInterceptor(kb))
            {
                Application.Run();
            }
        }
    }
}
