
using Mono.Cecil;

var managedDir = args[0];
var inputDir = args[1];

HashSet<string> modLibraries = [];

//Console.SetOut(new StreamWriter(File.OpenWrite("compatib.txt")));

foreach (var v in Directory.EnumerateFiles(inputDir, "*.dll", SearchOption.AllDirectories))
{
    using var ad = AssemblyDefinition.ReadAssembly(v, new()
    {
        ReadingMode = ReadingMode.Deferred
    });
    modLibraries.Add(ad.Name.Name.ToLower());
}

var resolver = new DefaultAssemblyResolver();
resolver.AddSearchDirectory(managedDir);

foreach (var v in Directory.EnumerateFiles(inputDir, "*.dll", SearchOption.AllDirectories))
{

    HashSet<string> missingType = [];

    using var ad = AssemblyDefinition.ReadAssembly(v, new()
    {
        AssemblyResolver = resolver
    });

    Console.WriteLine("#ASM:" + ad.Name.Name);

    var mm = ad.MainModule;
    foreach (var t in mm.GetTypeReferences())
    {
        try
        {
            if(t.Resolve() != null)
            {
                continue;
            }
            
        } catch (AssemblyResolutionException)
        {
           
        }

        missingType.Add(t.FullName);

        if(t.FullName.StartsWith("On.") || t.FullName.StartsWith("IL."))
        {
            continue;
        }

        var asmName = (AssemblyNameReference)t.Scope;

        if (!modLibraries.Contains(asmName.Name.ToLower()))
        {
            Console.WriteLine($"[Type]{t.FullName}, {asmName.Name}");
        }
    }

    foreach (var m in mm.GetMemberReferences())
    {
        var pt = m.DeclaringType?.FullName;
        if (string.IsNullOrEmpty(pt))
        {
            continue;
        }

        if (missingType.Contains(pt))
        {
            continue;
        }

        try
        {
            if(m.Resolve() != null)
            {
                continue;
            }
            if(m.DeclaringType != null)
            {
                if(m.DeclaringType.FullName.StartsWith("On."))
                {
                    continue;
                }
            }
        }
        catch (AssemblyResolutionException ex)
        {
            if(modLibraries.Contains(ex.AssemblyReference.Name.ToLower()))
            {
                continue;
            }
        }
        catch (ResolutionException)
        {
        }

        var kind = m switch
        {
            FieldReference => "Field",
            MethodReference => "Method",
            EventReference => "Event",
            PropertyReference => "Prop",
            _ => throw new NotSupportedException()
        };
        var asmName = m.DeclaringType?.Scope as AssemblyNameReference;

        Console.WriteLine($"[Member][{kind}]{m},{asmName?.Name}");
    
    } 
}

Console.Out.Flush();
