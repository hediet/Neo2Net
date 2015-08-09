using System.Windows.Forms;

namespace ConsoleApplicationNeoTest
{
    interface ISemanticKeyMap
    {
        SemanticKey GetSemanticKey(Keys key, Layer layer);
    }
}