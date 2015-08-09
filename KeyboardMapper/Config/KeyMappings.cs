using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [TymlObjectType]
    class KeyMappings
    {
        public LayerDefinition[] Layers { get; set; }

        public KeyMapping[] Mappings { get; set; }
    }
}