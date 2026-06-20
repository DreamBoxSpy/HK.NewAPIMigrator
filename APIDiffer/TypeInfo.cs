using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIDiffer
{

    class TypeInfo
    {
        [Flags]
        public enum LinkKind
        {
            Unchanged = 0,
            Rename = 1,
            MoveAssemly = 2
        }

        public required string AssemblyName { get; set; }
        public required string FullName { get; set; }
        public required TypeDefinition Definition { get; set; }

        public HashSet<string> Fields { get; set; } = [];
        public HashSet<string> Methods { get; set; } = [];

        public LinkKind LKind { get; set; }
        public TypeInfo? Link { get; set; }
        public bool IsLinked => Link != null;

        public void LinkTo(TypeInfo another, LinkKind kind)
        {
            if(Link != null)
            {
                if(Link == another)
                {
                    return;
                }
                throw new InvalidOperationException();
            }
            another.LKind = LKind;
            another.Link = this;
            Link = another;
            LKind = kind;
        }
    }

}
