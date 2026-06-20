
using APIDiffer;
using Mono.Cecil;
using Spectre.Console;
using System.Diagnostics;

var oldAPI = args[0];
var newAPIRoot = args[1];

using var oAPI = new APIInfo();
using var nAPI = new APIInfo();

oAPI.Read(AssemblyDefinition.ReadAssembly(oldAPI));

foreach(var v in Directory.EnumerateFiles(newAPIRoot, "*.dll", SearchOption.TopDirectoryOnly))
{
    try
    {
        nAPI.Read(AssemblyDefinition.ReadAssembly(v));
    }
    catch { }
}

// Resolve Unchanged

foreach(var v in nAPI.Types)
{
    if(v.IsLinked)
    {
        continue;
    }

    var oldType = oAPI.GetType(v.FullName);

    if(oldType != null && !oldType.IsLinked)
    {
        if(oldType.AssemblyName == v.AssemblyName)
        {
            v.LinkTo(oldType, TypeInfo.LinkKind.Unchanged);
        }
    }
}

// Resolve rename

foreach (var v in nAPI.Types)
{
    if (v.IsLinked)
    {
        continue;
    }

    var oldAsm = oAPI.Assemblies.FirstOrDefault(x => x.Name.Name == v.AssemblyName);

    if (oldAsm == null)
    {
        continue;
    }

    var oldTypeDef = oldAsm.MainModule.Types.FirstOrDefault(x => x.Name == v.Definition.Name);

    if(oldTypeDef == null)
    {
        continue;
    }

    var oldType = oAPI.TypeInfoMapping[oldTypeDef];
    v.LinkTo(oldType, TypeInfo.LinkKind.Rename);
}

// Resolve move assembly

foreach (var v in nAPI.Types)
{
    if (v.IsLinked)
    {
        continue;
    }

    var oldType = oAPI.GetType(v.FullName);

    if (oldType != null && !oldType.IsLinked)
    {
        v.LinkTo(oldType, TypeInfo.LinkKind.MoveAssemly);
    }
}

// Resolve move assembly & rename

foreach (var v in nAPI.Types)
{
    if (v.IsLinked)
    {
        continue;
    }

    var oldTypeDef = oAPI.Assemblies.SelectMany(x => x.MainModule.Types).FirstOrDefault(x => x.Name == v.Definition.Name);

    if (oldTypeDef == null)
    {
        continue;
    }

    var oldType = oAPI.TypeInfoMapping[oldTypeDef];
    v.LinkTo(oldType, TypeInfo.LinkKind.Rename | TypeInfo.LinkKind.MoveAssemly);
}



// Print

foreach(var v in nAPI.Types)
{
    if(v.AssemblyName.StartsWith("Mono") ||
        v.AssemblyName.StartsWith("MMHOOK_")
        )
    {
        continue;
    }

    if (v.IsLinked)
    {
        Debug.Assert(v.Link != null);
        if (v.LKind != TypeInfo.LinkKind.Unchanged)
        {
            AnsiConsole.MarkupLine($"[blue][[#]]{v.Link.FullName}, {v.Link.AssemblyName} -> {v.FullName}, {v.AssemblyName}[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[gray][[@]]{v.Link.FullName}, {v.Link.AssemblyName}[/]");
        }

        // Diff members

        var old = v.Link;

        foreach(var f in v.Fields)
        {
            if(!old.Fields.Contains(f))
            {
                AnsiConsole.MarkupLine($"   - [green][[+]]{f}[/]");
            }
        }
        foreach (var m in v.Methods)
        {
            if (!old.Methods.Contains(m))
            {
                AnsiConsole.MarkupLine($"   - [green][[+]]{m}[/]");
            }
        }
        foreach (var f in old.Fields)
        {
            if (!v.Fields.Contains(f))
            {
                AnsiConsole.MarkupLine($"   - [red][[-]]{f}[/]");
            }
        }
        foreach (var m in old.Methods)
        {
            if (!v.Methods.Contains(m))
            {
                AnsiConsole.MarkupLine($"   - [red][[-]]{m}[/]");
            }
        }
        continue;
    }

    
    AnsiConsole.MarkupLine($"[green][[+]]{v.FullName}, {v.AssemblyName}[/]");
}

foreach(var v in oAPI.Types)
{
    if (v.AssemblyName.StartsWith("Mono") ||
         v.AssemblyName.StartsWith("MMHOOK_")
         )
    {
        continue;
    }
    if (v.IsLinked)
    {
        continue;
    }

    AnsiConsole.MarkupLine($"[red][[-]]{v.FullName}, {v.AssemblyName}[/]");
}

