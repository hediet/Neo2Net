using System;
using System.Globalization;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    class Key : IEquatable<Key>
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

        public bool Equals(Key other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return KeyType == other.KeyType && Character == other.Character && KeyCode == other.KeyCode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Key) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) KeyType;
                hashCode = (hashCode*397) ^ Character.GetHashCode();
                hashCode = (hashCode*397) ^ (int) KeyCode;
                return hashCode;
            }
        }
    }
}