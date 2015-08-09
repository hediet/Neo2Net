using System.Globalization;
using System.Windows.Forms;

namespace ConsoleApplicationNeoTest
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

        public KeyType KeyType { get; private set; }

        public char Character { get; private set; }

        public Keys KeyCode { get; private set; }


        public override string ToString()
        {
            if (KeyType == KeyType.KeyCode)
                return KeyCode.ToString();
            
            return Character.ToString(CultureInfo.InvariantCulture);
        }
    }
}