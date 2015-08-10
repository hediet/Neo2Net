using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    [StringWrapper(typeof(CharSequence)), TymlUnionType]
    public abstract class KeyOrString
    {
        internal abstract SemanticKey ToSemanticKey();
    }

    [TymlObjectType, TymlName("Key")]
    public class KeyReference : KeyOrString
    {
        [CanBeImplicit]
        public string Name { get; set; }

        internal override SemanticKey ToSemanticKey()
        {
            return new SemanticKey(Name, null);
        }
    }

    [TymlStringType]
    public class CharSequence : KeyOrString
    {
        public CharSequence(string value) { Value = value; }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }

        internal override SemanticKey ToSemanticKey()
        {
            return new SemanticKey(null, Value);
        }
    }
}