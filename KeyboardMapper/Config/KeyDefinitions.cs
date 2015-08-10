using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [TymlObjectType]
    public class KeyDefinitions
    {
        [CanBeImplicit]
        public KeyDefinition[] Definitions { get; set; }
    }
}
