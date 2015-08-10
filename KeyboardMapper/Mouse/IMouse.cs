using System.Drawing;

namespace Hediet.KeyboardMapper.Mouse
{
    public interface IMouse
    {
        Point Position { get; set; }

        void SetButtonState(MouseButton button, KeyPressDirection direction);
    }

}