using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tyml.Serialization;

namespace ConsoleApplicationNeoTest.Config
{
    [TymlObjectType]
    class KeyDefinitions
    {
        [CanBeImplicit]
        public KeyDefinition[] Definitions { get; set; }
    }
}
