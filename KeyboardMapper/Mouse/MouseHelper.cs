namespace Hediet.KeyboardMapper.Mouse
{
    public static class MouseHelper
    {
        public static void Click(this IMouse mouse, MouseButton button)
        {
            mouse.SetButtonState(button, KeyPressDirection.Down);
            mouse.SetButtonState(button, KeyPressDirection.Up);
        }

        public static void DoubleClick(this IMouse mouse, MouseButton button)
        {
            mouse.Click(button);
            mouse.Click(button);
        }
    }
}