namespace Hediet.KeyboardMapper
{
    class Layer
    {
        public Layer(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}