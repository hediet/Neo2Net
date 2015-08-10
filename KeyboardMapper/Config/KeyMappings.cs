using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [TymlObjectType]
    public class KeyMappings
    {
        public LayerDefinition[] Layers { get; set; }

        public KeyMapping[] Mappings { get; set; }
    }
}