namespace ConsoleApplicationNeoTest
{
    class SemanticKey
    {
        public SemanticKey(string name, string text)
        {
            Name = name;
            Text = text;
        }

        public string Name { get; private set; }

        public string Text { get; private set; }

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