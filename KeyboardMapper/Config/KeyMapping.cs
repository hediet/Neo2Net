using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [TymlObjectType]
    public class KeyMapping
    {
        public string Layer { get; set; }
        public int ScanCode { get; set; }

        public int? VirtualCode { get; set; }

        public KeyOrString MapsTo { get; set; }
    }
}