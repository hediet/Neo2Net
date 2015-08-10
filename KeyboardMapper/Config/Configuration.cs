using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [TymlObjectType]
    public class Configuration
    {
        public KeyDefinition[] KeyDefinitions { get; set; }

        public LayerDefinition[] Layers { get; set; }

        public KeyMapping[] KeyMappings { get; set; }

        public CompositionDefinition[] CompositionDefinitions { get; set; }
    }
}