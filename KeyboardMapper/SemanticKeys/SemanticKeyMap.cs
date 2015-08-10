using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Hediet.KeyboardMapper.Config;
using Tyml.Serialization;

namespace Hediet.KeyboardMapper
{
    class SemanticKeyMap: ISemanticKeyMap, ILayerProvider
    {
        private readonly Dictionary<Tuple<string, Keys>, SemanticKey> vkeys = new Dictionary<Tuple<string, Keys>, SemanticKey>();
        private readonly Dictionary<Tuple<string, uint>, SemanticKey> keys = new Dictionary<Tuple<string, uint>, SemanticKey>();

        private readonly LayerDefinition[] layers;

        public SemanticKeyMap()
        {
            var keyMappings = TymlSerializerHelper.DeserializeFromFile<KeyMappings>("Data/KeyMappings.tyml"); 

            foreach (var m in keyMappings.Mappings)
            {
                if (m.VirtualCode != null)
                    vkeys[Tuple.Create(m.Layer, (Keys)m.VirtualCode)] = m.MapsTo.ToSemanticKey();
                else
                    keys[Tuple.Create(m.Layer, (uint)m.ScanCode)] = m.MapsTo.ToSemanticKey();
            }

            layers = keyMappings.Layers;

            ModifierKeys = layers.SelectMany(l => l.ModifierKeys)
                .SelectMany(item => item)
                .Select(item => item.ToSemanticKey())
                .Distinct()
                .ToArray();
        }

        public SemanticKey GetSemanticKey(Keys key, Layer layer)
        {
            SemanticKey result;
            if (!vkeys.TryGetValue(Tuple.Create(layer.Name, key), out result))
                keys.TryGetValue(Tuple.Create(layer.Name, KeysHelper.ConvertToScanCode(key)), out result);
            return result;
        }

        public Layer GetLayer(SemanticKey[] pressedKeys)
        {
            var keys = pressedKeys.Intersect(ModifierKeys).ToList();
            while (true)
            {
                var hs1 = new HashSet<string>(keys.Select(k => k.Name));

                foreach (var layer in layers)
                {
                    foreach (var mk in layer.ModifierKeys)
                    {
                        var hs = new HashSet<string>(mk.Select(k => k.ToSemanticKey().Name));
                        if (hs1.SetEquals(hs))
                            return new Layer(layer.Name);
                    }
                }

                keys.Remove(keys.Last());
            }
        }

        public SemanticKey[] ModifierKeys { get; private set; }
    }
}