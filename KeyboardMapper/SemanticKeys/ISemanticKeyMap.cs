using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    interface ISemanticKeyMap
    {
        SemanticKey GetSemanticKey(Keys key, Layer layer);
    }
}