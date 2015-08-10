using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Hediet.KeyboardMapper.Config;
using Tyml.ConsoleHelper;
using Tyml.Nodes.Immutable;
using Tyml.Serialization;

namespace ConsoleApplicationNeoTest.ConfigGenerator
{
    class ConfigGenerator2
    {
        public ConfigGenerator2()
        {
            var defs = TymlSerializerHelper.DeserializeFromFile<KeyDefinitions>(
                @"C:\Henning\Coding\Projects\Neo2Net\KeyboardMapper\Data\KeyDefinitions.tyml");

            var dict = defs.Definitions.ToDictionary(d => d.Name);

            var makeComposeContent = File.ReadAllText(@"Resources/makecompose.ahk");

            var r = new Regex("(?<isOptional>;\\s*)?DefineXKBSym\\(\"(?<name>.*)\"\\s*,\"U(?<value>.*)\"\\)");

            var exceptions = new HashSet<string> { "nobreakspace" };

            foreach (var match in r.Matches(makeComposeContent).OfType<Match>())
            {
                var name = match.Groups["name"].Value;
                var value = match.Groups["value"].Value;
                var isOptional = match.Groups["isOptional"].Value != "";

                var unicode = char.ConvertFromUtf32(int.Parse(value, NumberStyles.AllowHexSpecifier));

                KeyDefinition existingDef;
                if (dict.TryGetValue(name, out existingDef))
                {
                    if (existingDef.Text != null && existingDef.Text != unicode && !exceptions.Contains(name))
                    {
                        if (!isOptional)
                            throw new Exception();
                    }
                    else
                    {
                        exceptions.Remove(name);
                        existingDef.Text = unicode;
                    }
                }
                else
                {
                    existingDef = new KeyDefinition { Name = name, Text = unicode };
                    dict[existingDef.Name] = existingDef;
                }
            }

            defs.Definitions = dict.Values.ToArray();

            TymlSerializerHelper.SerializeToFile(defs,
                @"C:\Henning\Coding\Projects\Neo2Net\KeyboardMapper\Data\KeyDefinitions.tyml");
        }
    }
}