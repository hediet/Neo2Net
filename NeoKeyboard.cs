using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ConsoleApplicationNeoTest
{
    class NeoKeyboard : IKeyboard
    {
        private readonly IKeyboard baseKeyboard;

        private readonly Dictionary<Keys, Key>[] mappings = new Dictionary<Keys, Key>[6];

        public NeoKeyboard(IKeyboard baseKeyboard)
        {
            if (baseKeyboard == null) throw new ArgumentNullException("baseKeyboard");
            this.baseKeyboard = baseKeyboard;


            for (int i = 0; i < mappings.Length; i++)
            {
                mappings[i] = new Dictionary<Keys, Key>();
            }

            var lines = File.ReadAllText("keys.txt");

            var regex = new Regex(@"
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

            
            foreach (var match in regex.Matches(lines).OfType<Match>())
            {
                var scanCode = uint.Parse(match.Groups["scan"].Value);
                var name = match.Groups["name"].Value;
                var mods = new HashSet<string>();
                var unicode = match.Groups["unicode"].Value;

                foreach (string groupName in regex.GetGroupNames().Where(n => n.StartsWith("mod")))
                {
                    if (match.Groups[groupName].Value != "")
                        mods.Add(match.Groups[groupName].Value);
                }

                var sourceKey = KeysHelper.Convert(scanCode);

                Key key;
                Keys parsedKey;

                if (unicode != "")
                {
                    var c = char.ConvertFromUtf32(int.Parse(unicode, NumberStyles.AllowHexSpecifier)).First();
                    var k2 = KeysHelper.Convert(c);
                    key = k2 != null ? new Key(k2.Value) : new Key(c);
                }
                else if (Keys.TryParse(name, out parsedKey))
                {
                    key = new Key(parsedKey);
                }
                else
                {
                    if (name == "BackSpace")
                        key = new Key(Keys.Back);
                    else
                        key = null;
                }

                if (key != null)
                {
                    var sets = new[]
                    {
                        new HashSet<string> {},
                        new HashSet<string> {"shiftl"},
                        new HashSet<string> {"altgr"},
                        new HashSet<string> {"shiftl", "ctrll"}
                    };

                    int j = -1;
                    for (int i = 0; i < sets.Length; i++)
                        if (sets[i].SetEquals(mods))
                        {
                            j = i;
                            break;
                        }
                    if (j != -1)
                        mappings[j][sourceKey] = key;
                }
            }


            
        }


        private bool isShiftPressed;
        private bool isMod4Pressed;
        private bool isMod3Pressed;

        public void KeyEvent(Key key, KeyPressDirection pressDirection)
        {
            if (key.KeyCode == Keys.ShiftKey || key.KeyCode == Keys.LShiftKey || key.KeyCode == Keys.RShiftKey)
                isShiftPressed = pressDirection == KeyPressDirection.Down;

            if (key.KeyCode == Keys.RMenu || key.KeyCode == Keys.OemBackslash)
            {
                isMod4Pressed = pressDirection == KeyPressDirection.Down;
                return;
            }

            if (key.KeyCode == Keys.Capital || key.KeyCode == Keys.OemQuestion)
            {
                isMod3Pressed = pressDirection == KeyPressDirection.Down;
                return;
            }

            Key translatedKey;

            int level = 0;

            if (isShiftPressed && !isMod3Pressed && !isMod4Pressed)
                level = 1;

            if (!isShiftPressed && isMod3Pressed && !isMod4Pressed)
                level = 2;

            if (!isShiftPressed && !isMod3Pressed && isMod4Pressed)
                level = 3;
            
            if (mappings[level].TryGetValue(key.KeyCode, out translatedKey))
            {
                baseKeyboard.KeyEvent(translatedKey, pressDirection);
                return;
            }

            baseKeyboard.KeyEvent(key, pressDirection);
        }
    }
}