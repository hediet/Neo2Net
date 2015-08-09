using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tyml.Serialization;


namespace ConsoleApplicationNeoTest.Config
{
    [TymlObjectType]
    class CompositionDefinition
    {
        [CanBeImplicit]
        public KeyOrString[] Sequence { get; set; }

        [CanBeImplicit]
        public KeyOrString[] Result { get; set; }
    }
}
