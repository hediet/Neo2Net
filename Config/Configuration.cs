using Tyml.Serialization;

[assembly: TymlNamespaceDefinition("http://hediet.de/neo2-win", "ConsoleApplicationNeoTest.Config")]

namespace ConsoleApplicationNeoTest.Config
{
    [TymlObjectType]
    class Configuration
    {
        public KeyDefinition[] KeyDefinitions { get; set; }

        public LayerDefinition[] Layers { get; set; }

        public KeyMapping[] KeyMappings { get; set; }

        public CompositionDefinition[] CompositionDefinitions { get; set; }
    }
}