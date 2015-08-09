using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ConsoleApplicationNeoTest.Config;
using Tyml.Nodes;
using Tyml.Serialization;

namespace ConsoleApplicationNeoTest
{
    class SemanticKeyMap: ISemanticKeyMap, ILayerProvider
    {
        private readonly Dictionary<Tuple<string, uint>, SemanticKey> keys = new Dictionary<Tuple<string, uint>, SemanticKey>();

        private LayerDefinition[] layers;

        public SemanticKeyMap()
        {
            var keyMappings = TymlSerializer.DeserializeFromFile<KeyMappings>("Data/KeyMappings.tyml"); 

            foreach (var m in keyMappings.Mappings)
            {
                keys[Tuple.Create(m.Layer, (uint)m.ScanCode)] =
                    m.MapsTo.ToSemanticKey();
            }

            layers = keyMappings.Layers;
        }

        public SemanticKey GetSemanticKey(Keys key, Layer layer)
        {
            SemanticKey result;
            keys.TryGetValue(Tuple.Create(layer.Name, KeysHelper.ConvertToScanCode(key)), out result);
            return result;
        }

        public Layer GetLayer(SemanticKey[] pressedKeys)
        {
            var hs1 = new HashSet<string>(pressedKeys.Select(k => k.Name));

            foreach (var layer in layers)
            {
                foreach (var mk in layer.ModifierKeys)
                {
                    var hs = new HashSet<string>(mk.Select(k => k.ToSemanticKey().Name));
                    if (hs1.SetEquals(hs))
                        return new Layer(layer.Name);
                }
            }

            return null;
        }
    }
}