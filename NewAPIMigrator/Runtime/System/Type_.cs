using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NewAPIMigrator.Runtime.System
{
    public class Type_
    {
        public static MethodInfo GetMethod_1(Type self, string name)
        {
            return GetMethod_2(self, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
        }

        public static MethodInfo GetMethod_2(Type self, string name, BindingFlags flags)
        {
            try
            {
                return self.GetMethod(name, flags);
            }
            catch (AmbiguousMatchException)
            {
                var methods = self.GetMethods(flags);
                return methods.Where(x => x.Name == name).OrderBy(x => x.GetParameters().Length).FirstOrDefault();
            }
        }
        public static MethodInfo GetMethod_2(Type self, string name, Type[] args)
        {
            return self.GetMethod(name, args);
        }
    }
}
