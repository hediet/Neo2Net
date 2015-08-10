using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Hediet.KeyboardMapper.Config;
using Tyml.Serialization;

namespace ConsoleApplicationNeoTest.ConfigGenerator
{
    class ConfigGenerator3
    {
        public ConfigGenerator3()
        {
            const string file = "Resources/Compose/fast.module";  //"base.module";

            var compositions = new List<CompositionDefinition>();

            foreach (var line in File.ReadAllLines(file))
            {
                var realLine = line.Split('#')[0].Trim();

                if (realLine == "")
                    continue;

                var parts = realLine.Split(':');

                var sRegex = new Regex("<(.*?)>");

                var seq = sRegex.Matches(parts[0]).OfType<Match>().Select(m => m.Groups[1].Value).ToArray();

                seq = seq.Select(item => item == "Multi_key" ? "Compose" : item).ToArray();

                var rRegex = new Regex("\"(.*)\"");

                var result = rRegex.Match(parts[1]).Groups[1].Value;
                
                compositions.Add(new CompositionDefinition
                {
                    Sequence = seq.Select(s => new KeyReference {Name = s}).ToArray<KeyOrString>(),
                    Result = new KeyOrString[]{ new CharSequence(result) }
                });
            }

            var d = new CompositionDefinitions {Definitions = compositions.ToArray()};

            const string targetFile = @"C:\Henning\Coding\Projects\Neo2Net\KeyboardMapper\Data\CompositionDefinitions.tyml";
            
            TymlSerializerHelper.SerializeToFile(d, targetFile);
        }
        
    }
}