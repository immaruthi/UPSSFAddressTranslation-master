using Quincus.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.Application.CustomLogs
{

    public class LogServiceAssemblyReference : AssemblyReferenceBase
    {
        public override ImplementationType ImplementationType
        {
            get { return ImplementationType.APILog; }
        }
    }
}
