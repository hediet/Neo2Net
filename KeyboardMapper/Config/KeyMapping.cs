using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [TymlObjectType]
    class KeyMapping
    {
        public string Layer { get; set; }
        public int ScanCode { get; set; }

        public KeyOrString MapsTo { get; set; }
    }
}