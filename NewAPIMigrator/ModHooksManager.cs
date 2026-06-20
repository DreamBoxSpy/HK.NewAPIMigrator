using Modding;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace NewAPIMigrator
{
    public static class ModHooksManager
    {
        public static int disableModHooksCount = 0;

        public static bool CanCallModHooks() => disableModHooksCount == 0;

        private static readonly List<ILHook> ilhooks = [];
        public class DisableModHooks : IDisposable
        {
            private bool disposed = false;
            public DisableModHooks()
            {
                Interlocked.Increment(ref disableModHooksCount);
            }

            public void Dispose()
            {
                if(disposed)
                {
                    return;
                }
                disposed = true;
                Interlocked.Decrement(ref disableModHooksCount);
            }
        }

        public static void Init()
        {

            var m_CanCallModHooks = typeof(ModHooksManager).GetMethod(nameof(CanCallModHooks));

            foreach (var v in typeof(ModHooks).GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
            {
                if(v.ContainsGenericParameters)
                {
                    continue;
                }

                ilhooks.Add(new(v, ctx =>
                {
                    
                    var hookIsNull = ctx.Body.Instructions.FirstOrDefault(x => x.OpCode == OpCodes.Ldsfld &&
                        x.Operand is FieldReference fr && 
                        fr.DeclaringType.FullName == "Modding.ModHooks" &&
                        fr.FieldType.Resolve().BaseType.FullName == typeof(MulticastDelegate).FullName &&
                        x.Next.OpCode.FlowControl == FlowControl.Cond_Branch
                    );

                    if(hookIsNull == null)
                    {
                        return;
                    }
                    ctx.IL.InsertAfter(hookIsNull, Instruction.Create(OpCodes.And));
                    ctx.IL.InsertAfter(hookIsNull, Instruction.Create(OpCodes.Call, ctx.Import(m_CanCallModHooks)));
                    ctx.IL.InsertAfter(hookIsNull, Instruction.Create(OpCodes.Not));
                    ctx.IL.InsertAfter(hookIsNull, Instruction.Create(OpCodes.Ceq));
                    ctx.IL.InsertAfter(hookIsNull, Instruction.Create(OpCodes.Ldnull));
                }));

            }
        }
    }
}
