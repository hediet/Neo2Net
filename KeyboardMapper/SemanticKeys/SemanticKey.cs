using System;

namespace Hediet.KeyboardMapper
{
    class SemanticKey : IEquatable<SemanticKey>
    {
        public SemanticKey(string name, string text)
        {
            Name = name;
            Text = text;
        }

        public string Name { get; }

        public string Text { get; }

        public override string ToString()
        {
            var result = "";
            if (Name != null)
                result += Name;

            if (Text != null)
            {
                if (Name != null)
                    result += ": ";
                result += Text;
            }

            return result;
        }

        public bool Equals(SemanticKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SemanticKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (Text != null ? Text.GetHashCode() : 0);
            }
        }
    }
}