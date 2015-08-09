using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [TymlObjectType]
    class KeyDefinitions
    {
        [CanBeImplicit]
        public KeyDefinition[] Definitions { get; set; }
    }
}
