using Tyml.Serialization;

namespace ConsoleApplicationNeoTest.Config
{
    enum QwertzModifier
    {
        Shift, Alt, Strg
    }

    [TymlObjectType]
    class KeyDefinition
    {
        [CanBeImplicit]
        public string Name { get; set; }
        public string Text { get; set; }

        public int? QwertzScanCode { get; set; }
        public int? QwertzVirtualKeyCode { get; set; }

        public QwertzModifier[] Modifiers { get; set; }
    }
}