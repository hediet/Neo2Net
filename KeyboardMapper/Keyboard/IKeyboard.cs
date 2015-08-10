namespace Hediet.KeyboardMapper
{
    interface IKeyboard
    {
        void HandleKeyEvent(Key key, KeyPressDirection pressDirection);
    }
}