using System;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Migrator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    internal class MigrateIgnore : Attribute
    {
    }
}
