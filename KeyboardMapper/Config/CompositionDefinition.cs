using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [TymlObjectType, TymlName("Composition")]
    public class CompositionDefinition
    {
        [CanBeImplicit]
        public KeyOrString[] Sequence { get; set; }

        [CanBeImplicit]
        public KeyOrString[] Result { get; set; }
    }
}
