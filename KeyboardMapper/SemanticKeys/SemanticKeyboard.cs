using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Hediet.KeyboardMapper.Config;
using Hediet.KeyboardMapper.Mouse;
using Tyml.Serialization;

namespace Hediet.KeyboardMapper
{
    class SemanticKeyboard : ISemanticKeyboard
    {
        private readonly IDictionary<string, KeyDefinition> keyDefinitions = new Dictionary<string, KeyDefinition>();
        private readonly IKeyboard targetKeyboard;

        public SemanticKeyboard(IKeyboard targetKeyboard)
        {
            this.targetKeyboard = targetKeyboard;

            var defs = TymlSerializerHelper.DeserializeFromFile<KeyDefinitions>("Data/KeyDefinitions.tyml");

            foreach (var def in defs.Definitions)
                keyDefinitions[def.Name] = def;

            new Thread(UpdateLoop) { IsBackground = true }.Start();
        }

        private readonly List<SemanticKey> pressedKeys = new List<SemanticKey>();
        private SemanticKey[] pressedKeys2 = new SemanticKey[0];

        private static Size Mul(Size size, int factor)
        {
            return new Size(size.Width * factor, size.Height * factor);
        }

        private void UpdateLoop()
        {
            var dict = new Dictionary<SemanticKey, Size>() {
                {new SemanticKey("MouseUp", null), new Size(0, -1)},
                {new SemanticKey("MouseRight", null), new Size(1, 0)},
                {new SemanticKey("MouseDown", null), new Size(0, 1)},
                {new SemanticKey("MouseLeft", null), new Size(-1, 0)}
            };

            var times = new Dictionary<SemanticKey, DateTime>();
            var mouse = new WindowsMouse();

            while (true)
            {
                Thread.Sleep(10);

                foreach (var key in dict.Keys)
                {
                    if (pressedKeys2.Contains(key) && !times.ContainsKey(key))
                        times[key] = DateTime.Now;
                    if (!pressedKeys2.Contains(key) && times.ContainsKey(key))
                        times.Remove(key);
                }

                var delta = new Size(0, 0);

                foreach (var kv in times)
                {
                    int factor = 15;
                    if (DateTime.Now - kv.Value < TimeSpan.FromSeconds(1))
                        factor = (int)Math.Ceiling((((DateTime.Now - kv.Value).TotalSeconds) * 15.0) / 1.0);

                    delta += Mul(dict[kv.Key], factor);
                }
                if (delta != Size.Empty)
                    mouse.Position += delta;
            }
        }

        public void HandleKeyEvent(SemanticKey semanticKey, KeyPressDirection pressDirection)
        {
            var firstPressEvent = !pressedKeys.Remove(semanticKey);

            if (pressDirection == KeyPressDirection.Down)
                pressedKeys.Add(semanticKey);

            pressedKeys2 = pressedKeys.ToArray();

            var lshiftKey = new SemanticKey("ShiftL", null);
            var rshiftKey = new SemanticKey("ShiftR", null);

            
            if (semanticKey.Name == "MouseLeftClick")
            {
                var m = new WindowsMouse();
                if (pressDirection == KeyPressDirection.Down)
                    m.SetButtonState(MouseButton.Left, KeyPressDirection.Down);
                else
                    m.SetButtonState(MouseButton.Left, KeyPressDirection.Up);
                return;
            }


            KeyDefinition kd;
            if (semanticKey.Name != null && keyDefinitions.TryGetValue(semanticKey.Name, out kd))
            {
                var isShiftPressed = pressedKeys.Contains(lshiftKey) || pressedKeys.Contains(rshiftKey);
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
                    targetKeyboard.HandleKeyEvent(new Key((Keys)kd.QwertzVirtualKeyCode), pressDirection);
                else if (kd.Text != null)
                    targetKeyboard.HandleKeyEvent(new Key(kd.Text.First()), pressDirection);
            }
            else if (semanticKey.Text != null)
                targetKeyboard.HandleKeyEvent(new Key(semanticKey.Text.First()), pressDirection);

        }
    }
}