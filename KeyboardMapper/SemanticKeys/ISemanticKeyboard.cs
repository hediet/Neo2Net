namespace Hediet.KeyboardMapper
{
    interface ISemanticKeyboard
    {
        void HandleKeyEvent(SemanticKey key, KeyPressDirection pressDirection);
    }
}