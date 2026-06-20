using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIDiffer
{
    internal class APIInfo : IDisposable
    {
        public Dictionary<TypeDefinition, TypeInfo> TypeInfoMapping { get; set; } = [];
        public List<TypeInfo> Types { get; set; } = [];
        public List<AssemblyDefinition> Assemblies { get; set; } = [];

        public TypeInfo? GetType(string type)
        {
            var t = Assemblies.Select(x => x.MainModule.GetType(type)).FirstOrDefault(x => x != null);
            if(t == null)
            {
                return null;
            }
            return TypeInfoMapping[t];
        }

        public void Dispose()
        {
            foreach(var v in Assemblies)
            {
                v.Dispose();
            }
        }

        private static string GetMethodName(MethodDefinition md)
        {
            return $"{(md.IsStatic ? "static" : "")} {md.ReturnType.Name} {md.Name}({ string.Join(',', md.Parameters.Select(x => x.ParameterType.Name)) })"
                .Replace("[", "[[")
                .Replace("]", "]]");
        }

        private static string GetFieldName(FieldDefinition fd)
        {
            return $"{(fd.IsStatic ? "static" : "")} {fd.FieldType.Name} {fd.Name}"
                .Replace("[", "[[")
                .Replace("]", "]]");
        }

        public void Read(AssemblyDefinition assembly)
        {
            Assemblies.Add(assembly);
            foreach (var v in assembly.MainModule.Types)
            {
                var inf = new TypeInfo()
                {
                    Definition = v,
                    FullName = v.FullName,
                    AssemblyName = v.Module.Assembly.Name.Name,
                    Methods = [.. v.Methods.Select(x => GetMethodName(x))],
                    Fields = [.. v.Fields.Select(x => GetFieldName(x))]
                };

                Types.Add(inf);
                TypeInfoMapping.Add(v, inf);
            }
        }
    }
}
