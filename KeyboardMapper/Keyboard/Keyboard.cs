using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hediet.KeyboardMapper.Config;
using Tyml.Serialization;

namespace Hediet.KeyboardMapper
{
    class Keyboard : IKeyboard
    {
        private readonly IKeyboard targetKeyboard;
        private readonly ISemanticKeyboard semanticKeyboard;
        private readonly ISemanticKeyMap keyMap;
        private readonly ILayerProvider layerProvider;
        

        public Keyboard(IKeyboard targetKeyboard, ISemanticKeyboard semanticKeyboard)
        {
            this.targetKeyboard = targetKeyboard;
            this.semanticKeyboard = semanticKeyboard;
            var k = new SemanticKeyMap();
            keyMap = k;
            layerProvider = k;
            
            var defs = TymlSerializerHelper.DeserializeFromFile<CompositionDefinitions>("Data/CompositionDefinitions.tyml");

            foreach (var def in defs.Definitions)
            {
                var path = new List<SemanticKey>();
                var i = 0;
                foreach (var s in def.Sequence)
                {
                    i++;
                    path.Add(s.ToSemanticKey());

                    if (i < def.Sequence.Length)
                    {
                        keySequences[path.ToArray()] = null;
                    }
                    else
                        keySequences[path.ToArray()] = def.Result.First().ToSemanticKey();
                }
            }

            modifierKeys = new HashSet<SemanticKey>(layerProvider.ModifierKeys);
        }


        private readonly HashSet<SemanticKey> modifierKeys = new HashSet<SemanticKey>();

        private readonly List<SemanticKey> pressedSemanticKeys = new List<SemanticKey>();
        private readonly Dictionary<Keys, Tuple<SemanticKey, Layer>> pressedKeys = new Dictionary<Keys, Tuple<SemanticKey, Layer>>();

        private readonly HashSet<SemanticKey> ignoreNextKeyUps = new HashSet<SemanticKey>();

        private readonly Dictionary<SemanticKey[], SemanticKey> keySequences = new Dictionary<SemanticKey[], SemanticKey>(new ArrayComparer<SemanticKey>());
        private readonly List<SemanticKey> currentKeySequence = new List<SemanticKey>();
        private SemanticKey sequenceClosure;
        private SemanticKey sequenceResult;

        private void SemanticKeyEvent(SemanticKey key, KeyPressDirection pressDirection)
        {
            if (key.Name == "ToggleMod7" && pressDirection == KeyPressDirection.Down)
            {
                if (!pressedSemanticKeys.Contains(new SemanticKey("Mod7", null)))
                    pressedSemanticKeys.Add(new SemanticKey("Mod7", null));
            }
            if (key.Name == "Mod4" && pressDirection == KeyPressDirection.Up)
                pressedSemanticKeys.Remove(new SemanticKey("Mod7", null));

            semanticKeyboard.HandleKeyEvent(key, pressDirection);
        }

        public void HandleKeyEvent(Key key, KeyPressDirection pressDirection)
        {
            SemanticKey semanticKey = null;
            Layer layer = null;


            Tuple<SemanticKey, Layer> res;
            if (pressedKeys.TryGetValue(key.KeyCode, out res))
            {
                pressedKeys.Remove(key.KeyCode);
                semanticKey = res.Item1;
                layer = res.Item2;
                if (semanticKey != null)
                    pressedSemanticKeys.Remove(semanticKey);
            }

            if (pressDirection == KeyPressDirection.Down)
            {
                layer = layerProvider.GetLayer(pressedSemanticKeys.ToArray());
                semanticKey = keyMap.GetSemanticKey(key.KeyCode, layer);
                
                pressedKeys[key.KeyCode] = Tuple.Create(semanticKey, layer);
                if (!pressedSemanticKeys.Contains(semanticKey))
                    pressedSemanticKeys.Add(semanticKey);
            }

            Console.Write("{0}: ", pressDirection);

            if (semanticKey != null)
            {
                Console.Write("{0}, Layer {1} ", semanticKey, layer);

                if (semanticKey.Equals(sequenceClosure))
                { 
                    Console.Write("Send sequence result {0}", sequenceResult);

                    if (pressDirection == KeyPressDirection.Up)
                        sequenceClosure = null;
                    SemanticKeyEvent(sequenceResult, pressDirection);
                }
                else if (pressDirection == KeyPressDirection.Down)
                {
                    if (modifierKeys.Contains(semanticKey) && currentKeySequence.Count > 0)
                    {
                        Console.Write("Ignoring modifier key {0} in sequence", semanticKey);
                        ignoreNextKeyUps.Add(semanticKey);
                    }
                    else
                    {
                        currentKeySequence.Add(semanticKey);
                        var path = currentKeySequence.ToArray();

                        if (keySequences.TryGetValue(path, out sequenceResult))
                        {
                            if (sequenceResult != null)
                            {
                                sequenceClosure = semanticKey;
                                currentKeySequence.Clear();
                                SemanticKeyEvent(sequenceResult, pressDirection);
                                Console.Write("Complete sequence '{0}' with {1}", string.Join("->", (IEnumerable<object>)path), semanticKey);
                            }
                            else
                            {
                                ignoreNextKeyUps.Add(semanticKey);
                                Console.Write("Continue sequence '{0}'", string.Join("->", (IEnumerable<object>)path));
                            }
                        }
                        else
                        {
                            SemanticKeyEvent(semanticKey, pressDirection);

                            if (currentKeySequence.Count > 1)
                                Console.Write(" unknown sequence '{0}'", string.Join("->", (IEnumerable<object>)path));
                            currentKeySequence.Clear();
                        }
                    }
                }
                else
                {
                    if (ignoreNextKeyUps.Contains(semanticKey))
                        ignoreNextKeyUps.Remove(semanticKey);
                    else
                        SemanticKeyEvent(semanticKey, pressDirection);
                }
            }
            else
            {
                try
                {
                    targetKeyboard.HandleKeyEvent(key, pressDirection);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.Write("Unmapped key");
            }

            if (key.KeyCode == Keys.RMenu && pressDirection == KeyPressDirection.Up) // maybe there is a better way
                targetKeyboard.HandleKeyEvent(new Key(Keys.LControlKey), pressDirection);


            Console.Write(" (scancode: {0}, virtualcode: {1} ({2}))", KeysHelper.ConvertToScanCode(key.KeyCode), (int)key.KeyCode, key.KeyCode);

            Console.WriteLine();
        }
    }
}