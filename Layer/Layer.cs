namespace ConsoleApplicationNeoTest
{
    class Layer
    {
        public Layer(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}