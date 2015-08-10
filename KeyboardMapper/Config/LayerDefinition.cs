using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [TymlObjectType, TymlName("Layer")]
    public class LayerDefinition
    {
        public string Name { get; set; }

        public KeyOrString[][] ModifierKeys { get; set; }
    }
}