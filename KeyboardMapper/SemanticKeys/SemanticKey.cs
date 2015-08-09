namespace Hediet.KeyboardMapper
{
    class SemanticKey
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
    }
}