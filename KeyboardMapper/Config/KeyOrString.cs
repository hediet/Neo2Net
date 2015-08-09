using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [StringWrapper(typeof(CharSequence)), TymlUnionType]
    abstract class KeyOrString
    {
        public abstract SemanticKey ToSemanticKey();
    }

    [TymlObjectType, TymlName("Key")]
    class KeyReference : KeyOrString
    {
        [CanBeImplicit]
        public string Name { get; set; }

        public override SemanticKey ToSemanticKey()
        {
            return new SemanticKey(Name, null);
        }
    }

    [TymlStringType]
    class CharSequence : KeyOrString
    {
        public CharSequence(string value) { Value = value; }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }

        public override SemanticKey ToSemanticKey()
        {
            return new SemanticKey(null, Value);
        }
    }
}