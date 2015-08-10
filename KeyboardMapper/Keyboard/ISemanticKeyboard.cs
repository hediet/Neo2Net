namespace Hediet.KeyboardMapper
{
    interface ISemanticKeyboard
    {
        void KeyEvent(SemanticKey key, KeyPressDirection pressDirection);
    }
}