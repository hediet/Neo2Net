using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ConsoleApplicationNeoTest.Config;
using Tyml.Serialization;

namespace ConsoleApplicationNeoTest
{
    class Keyboard : IKeyboard
    {
        private readonly IKeyboard targetKeyboard;
        private ISemanticKeyMap keyMap;
        private ILayerProvider layerProvider;

        private IDictionary<string, KeyDefinition> keyDefinitions = new Dictionary<string, KeyDefinition>();

        public Keyboard(IKeyboard targetKeyboard)
        {
            this.targetKeyboard = targetKeyboard;
            var k = new SemanticKeyMap();
            keyMap = k;
            layerProvider = k;

            var defs = TymlSerializer.DeserializeFromFile<KeyDefinitions>("Data/KeyDefinitions.tyml");

            foreach (var def in defs.Definitions)
                keyDefinitions[def.Name] = def;
        }

        private readonly List<Keys> pressedKeys = new List<Keys>();
        private readonly Dictionary<Keys, Tuple<SemanticKey, Layer>> keys = new Dictionary<Keys, Tuple<SemanticKey, Layer>>();

        public void KeyEvent(Key key, KeyPressDirection pressDirection)
        {
            pressedKeys.Remove(key.KeyCode);
            SemanticKey semanticKey = null;
            Layer layer = null;

            if (pressDirection == KeyPressDirection.Down)
            {
                pressedKeys.Add(key.KeyCode);

                var semanticPressedKeys = new List<SemanticKey>();

                foreach (var k in pressedKeys)
                {
                    var newLayer = layerProvider.GetLayer(semanticPressedKeys.ToArray());
                    if (newLayer != null)
                        layer = newLayer;

                    if (layer == null)
                        break;

                    semanticKey = keyMap.GetSemanticKey(k, layer);

                    if (semanticKey != null)
                        semanticPressedKeys.Add(semanticKey);
                }

                keys[key.KeyCode] = Tuple.Create(semanticKey, layer);
            }
            else
            {
                Tuple<SemanticKey, Layer> res;
                if (keys.TryGetValue(key.KeyCode, out res))
                {
                    semanticKey = res.Item1;
                    layer = res.Item2;
                }
            }

            if (semanticKey != null)
            {
                KeyDefinition kd;
                if (semanticKey.Name != null && keyDefinitions.TryGetValue(semanticKey.Name, out kd))
                {
                    var isShiftPressed = pressedKeys.Contains(Keys.LShiftKey) || pressedKeys.Contains(Keys.RShiftKey);
                    var modifierMatch = true;
                    if (kd.Modifiers != null)
                    {
                        var set1 = new HashSet<QwertzModifier>(kd.Modifiers);
                        var set2 = new HashSet<QwertzModifier>();
                        if (isShiftPressed)
                            set2.Add(QwertzModifier.Shift);
                        modifierMatch = set1.SetEquals(set2);
                    }

                    if (kd.QwertzVirtualKeyCode != null && modifierMatch)
                        targetKeyboard.KeyEvent(new Key((Keys)kd.QwertzVirtualKeyCode), pressDirection);
                    else if (kd.Text != null)
                        targetKeyboard.KeyEvent(new Key(kd.Text.First()), pressDirection);
                }
                else if (semanticKey.Text != null)
                    targetKeyboard.KeyEvent(new Key(semanticKey.Text.First()), pressDirection);
            }
            else
                targetKeyboard.KeyEvent(key, pressDirection);

            Console.WriteLine("{0} {1} (Layer {2}, scancode: {3}, virtualcode: {4})", 
                pressDirection, semanticKey, layer, KeysHelper.ConvertToScanCode(key.KeyCode), (int)key.KeyCode);

        }
    }
}