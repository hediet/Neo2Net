using Tyml.Serialization;

namespace ConsoleApplicationNeoTest.Config
{
    [TymlObjectType]
    class KeyMappings
    {
        public LayerDefinition[] Layers { get; set; }

        public KeyMapping[] Mappings { get; set; }
    }
}