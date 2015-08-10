using System;

namespace Hediet.KeyboardMapper
{
    class DumpKeyboard : IKeyboard
    {
        public void HandleKeyEvent(Key key, KeyPressDirection pressDirection)
        {
            Console.WriteLine(pressDirection + " " + key + " scancode: " + KeysHelper.ConvertToScanCode(key.KeyCode));
        }
    }
}