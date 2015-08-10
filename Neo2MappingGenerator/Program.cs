using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ConsoleApplicationNeoTest.ConfigGenerator;
using Hediet.KeyboardMapper.Config;
using Tyml.Serialization;

namespace Neo2MappingGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var obj =
                TymlSerializerHelper.DeserializeFromFile<KeyDefinitions>(
                    @"C:\Henning\Coding\Projects\Neo2Net\KeyboardMapper\Data\KeyDefinitions.tyml");

            XmlSerializer s = new XmlSerializer(typeof(KeyDefinitions));
            var sw = new StringWriter();
            s.Serialize(sw, obj);

            Console.WriteLine(sw.ToString());

            Console.ReadLine();

            /*
            Action<string> format = f =>
            {
                var s = new TymlSerializer();
                var o = s.Deserialize<object>(TymlParser.ParseFile(f).Root);
                File.WriteAllText(f, s.SerializeToDocument(o).Format().Text);
            };

            format(@"C:\Henning\Coding\Projects\Neo2Net\Data\KeyDefinitions.tyml");
            format(@"C:\Henning\Coding\Projects\Neo2Net\Data\KeyMappings.tyml");

            return;
            */
            //new ConfigGenerator.ConfigGenerator();

            //new ConfigGenerator3();
        }
    }
}
