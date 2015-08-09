using Tyml.Serialization;

namespace ConsoleApplicationNeoTest.Config
{
    [TymlObjectType, TymlName("Layer")]
    class LayerDefinition
    {
        public string Name { get; set; }

        public KeyOrString[][] ModifierKeys { get; set; }
    }
}