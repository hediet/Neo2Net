using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [TymlObjectType]
    class CompositionDefinition
    {
        [CanBeImplicit]
        public KeyOrString[] Sequence { get; set; }

        [CanBeImplicit]
        public KeyOrString[] Result { get; set; }
    }
}
