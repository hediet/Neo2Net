using System;

namespace Hediet.KeyboardMapper
{
    class SemanticDumpKeyboard: ISemanticKeyboard
    {
        public void HandleKeyEvent(SemanticKey key, KeyPressDirection pressDirection)
        {
            Console.WriteLine(pressDirection + " " + key);
        }
    }
}