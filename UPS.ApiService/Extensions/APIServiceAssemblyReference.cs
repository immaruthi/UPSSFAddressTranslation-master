using Quincus.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtService.Extensions
{
    public class APIServiceAssemblyReference : AssemblyReferenceBase
    {
        public override ImplementationType ImplementationType
        {
            get { return ImplementationType.APIServices; }
        }
    }
}
