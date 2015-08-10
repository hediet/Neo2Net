using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hediet.KeyboardMapper;
using Hediet.KeyboardMapper.Config;
using Tyml.Nodes.Immutable;
using Tyml.Serialization;

namespace ConsoleApplicationNeoTest.ConfigGenerator
{
    class ConfigGenerator
    {
        private readonly Dictionary<string, KeyDefinition> keyDefinitions = new Dictionary<string, KeyDefinition>();
        private readonly List<KeyMapping> keyMappings = new List<KeyMapping>();

        private readonly Dictionary<HashSet<string>, int> levels = new Dictionary<HashSet<string>, int>
                {
                    { new HashSet<string>(), 1 },
                    { new HashSet<string> { "shiftl" }, 2 },
                    //{ new HashSet<string> { "shiftr" }, 2 },
                    { new HashSet<string> { "altgr" }, 3 },
                    { new HashSet<string> { "ctrll" }, 4 },
                    //{ new HashSet<string> { "shiftl", "ctrll" }, 4 },
                    //{ new HashSet<string> { "shiftr", "ctrll" }, 4 },
                    { new HashSet<string> { "shiftl", "altgr" }, 5 },
                    //{ new HashSet<string> { "shiftr", "altgr" }, 5 },
                    { new HashSet<string> { "ctrll", "altgr" }, 6 }
                };


        public ConfigGenerator()
        {
            Gen();

            var layers = new[]
            {
                new LayerDefinition { Name = "1", ModifierKeys = new KeyOrString[][] { } },
                new LayerDefinition { Name = "2", ModifierKeys = new[] { new KeyOrString[] { new KeyReference { Name = "Shift" } } }},
                new LayerDefinition { Name = "3", ModifierKeys = new[] { new KeyOrString[] { new KeyReference { Name = "Mod3" } } }},
                new LayerDefinition { Name = "4", ModifierKeys = new[] { new KeyOrString[] { new KeyReference { Name = "Mod4" } } }},
                new LayerDefinition { Name = "5", ModifierKeys = new[] { new KeyOrString[] { new KeyReference { Name = "Shift" }, new KeyReference { Name = "Mod3" } } }},
                new LayerDefinition { Name = "6", ModifierKeys = new[] { new KeyOrString[] { new KeyReference { Name = "Mod3" }, new KeyReference { Name = "Mod4" } } }}
            };

            var serializer = new TymlSerializer();

            var p = "";//@"C:\Henning\Coding\Projects\Neo2Net\";
        
            var mappings = new KeyMappings { Mappings = keyMappings.ToArray(), Layers = layers };
            File.WriteAllText(p + "Data\\KeyMappings.tyml", serializer.SerializeToDocument(mappings).Format().Text);

            var definitions = new KeyDefinitions { Definitions = keyDefinitions.Values.ToArray() };
            File.WriteAllText(p  +"Data\\KeyDefinitions.tyml", serializer.SerializeToDocument(definitions).Format().Text);

        }


        private Regex GetRegex()
        {
            return new Regex(@"
                ^

                (?: \ *  (?<mod1>[^\s\#]+) \ + )?
                (?: \ *  (?<mod2>[^\s\#]+) \ + )?
                (?: \ *  (?<mod3>[^\s\#]+) \ + )?
                (?: \ *  (?<mod4>[^\s\#]+) \ + )?
                (?: \ *  keycode \ + )
                (?: \ *  (?<scan>[^\s\#]+)  )
                \ * =
                (?: \ *  (?<name>[^\s\#]+)  )

                \ *
                (\#
                [^\[\n]*

                (\[U\+(?<unicode>[0-9A-F]{4})\])?
                .*
                )?

                $

                ", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

        }


        private KeyDefinition GetOrCreateKeyDefinition(string name, char? unicodeChar)
        {
            KeyDefinition existingKeyDef;
            if (!keyDefinitions.TryGetValue(name, out existingKeyDef))
            {
                existingKeyDef = new KeyDefinition
                {
                    Name = name,
                    Text = unicodeChar?.ToString()
                };

                if (unicodeChar != null)
                {
                    var kk = KeysHelper.Convert(unicodeChar.Value);
                    if (kk != Keys.None)
                    {
                        var qmods = new List<QwertzModifier>();
                        if (kk.HasFlag(Keys.Shift))
                            qmods.Add(QwertzModifier.Shift);
                        if (kk.HasFlag(Keys.Alt))
                            qmods.Add(QwertzModifier.Alt);

                        kk = kk & ~Keys.Shift & ~Keys.Alt;

                        var scanCode2 = KeysHelper.ConvertToScanCode(kk);
                        //existingKeyDef.QwertzScanCode = (int)scanCode2;
                        existingKeyDef.QwertzVirtualKeyCode = (int)kk;
                        existingKeyDef.Modifiers = qmods.ToArray();
                    }
                }


                keyDefinitions[name] = existingKeyDef;
            }

            return existingKeyDef;
        }

        private void Gen()
        {
            var lines = File.ReadAllText("Data/keys.txt");
            var regex = GetRegex();

            foreach (var match in regex.Matches(lines).OfType<Match>())
            {
                var scanCode = uint.Parse(match.Groups["scan"].Value);
                var name = match.Groups["name"].Value;
                var mods = new HashSet<string>();
                var unicode = match.Groups["unicode"].Value;
                var c = unicode == "" ? null :
                    (char?)char.ConvertFromUtf32(int.Parse(unicode, NumberStyles.AllowHexSpecifier)).First();


                foreach (var groupName in regex.GetGroupNames().Where(n => n.StartsWith("mod"))
                    .Where(groupName => match.Groups[groupName].Value != ""))
                    mods.Add(match.Groups[groupName].Value);

                var sourceKey = KeysHelper.ConvertFromScanCode(scanCode);

                var mapping = new KeyMapping();

                if (!name.StartsWith("U+"))
                {
                    GetOrCreateKeyDefinition(name, c);
                    mapping.MapsTo = new KeyReference() { Name = name };
                }
                else
                {
                    mapping.MapsTo = new CharSequence(c.Value.ToString());
                }

                var kv = levels.FirstOrDefault(k => k.Key.SetEquals(mods));
                if (kv.Key == null)
                    continue;

                mapping.Layer = kv.Value.ToString();

                mapping.ScanCode = (int)scanCode;

                keyMappings.Add(mapping);
            }
        }
    }
}
