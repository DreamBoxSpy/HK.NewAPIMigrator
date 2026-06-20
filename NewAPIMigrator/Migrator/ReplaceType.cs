using System;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Migrator
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class ReplaceType : Attribute
    {
    }
}
