using System.Globalization;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    class Key
    {
        public Key(char character)
        {
            KeyType = KeyType.Character;
            Character = character;
        }

        public Key(Keys keyCode)
        {
            KeyType = KeyType.KeyCode;
            KeyCode = keyCode;
        }

        public KeyType KeyType { get; }

        public char Character { get; }

        public Keys KeyCode { get; }


        public override string ToString()
        {
            if (KeyType == KeyType.KeyCode)
                return KeyCode.ToString();
            
            return Character.ToString(CultureInfo.InvariantCulture);
        }
    }
}