using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [TymlObjectType]
    public class CompositionDefinitions
    {
        public CompositionDefinition[] Definitions { get; set; }
    }
}