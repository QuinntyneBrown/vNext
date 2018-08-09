using System;
using vNext.Core.Interfaces;

namespace vNext.Core.Common
{
    public class MachineDateTime : IDateTime
    {
        public DateTime UtcNow {  get { return System.DateTime.UtcNow; } }
    }
}
