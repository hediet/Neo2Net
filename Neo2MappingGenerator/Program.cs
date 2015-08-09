using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo2MappingGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
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

        }
    }
}
