using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace AssemblyPatcher;

public class PatcherTask : Task
{
    [Required]
    public ITaskItem[] AssemblyNames { get; set; }

    [Required]
    public ITaskItem[] AttributeAssemblyNames { get; set; }

    [Required]
    public string IntermediateOutputPath { get; set; }

    public ITaskItem[] RemoveSealed { get; set; }

    [Required]
    public ITaskItem[] SourceReferences { get; set; }

    public override bool Execute()
    {
        if (SourceReferences == null)
            throw new ArgumentNullException(nameof(SourceReferences));

        Directory.CreateDirectory(IntermediateOutputPath);

        var assemblies = new HashSet<string>();
        if (AssemblyNames != null)
            assemblies.UnionWith(AssemblyNames.Select(a => a.ItemSpec));

        if (RemoveSealed != null)
            assemblies.UnionWith(RemoveSealed.Select(a => a.ItemSpec));

        foreach (string target in assemblies)
        {
            ITaskItem assembly = SourceReferences.FirstOrDefault(x => Path.GetFileNameWithoutExtension(GetFullFilePath(IntermediateOutputPath, x.ItemSpec)) == target);
            if (assembly == null)
                continue;

            string assemblyPath = GetFullFilePath(IntermediateOutputPath, assembly.ItemSpec);
            string targetAssemblyPath = Path.Combine(IntermediateOutputPath, Path.GetFileName(assemblyPath));
            var module = ModuleDefMD.Load(assemblyPath);

            // Add InternalsVisibleTo.
            if (AssemblyNames != null)
            {
                foreach (ITaskItem item in AssemblyNames.Where(x => x.ItemSpec == target))
                {
                    string assemblyName = item.GetMetadata("AssemblyName");
                    if (!string.IsNullOrEmpty(assemblyName))
                        AddInternalsVisibleToAttribute(module, assemblyName, targetAssemblyPath);
                }
            }

            // Remove sealed.
            if (RemoveSealed != null && RemoveSealed.Any(x => x.ItemSpec == target))
                RemoveSealedFromTypes(module, targetAssemblyPath);

            module.Write(targetAssemblyPath);
        }

        return true;
    }

    private void AddInternalsVisibleToAttribute(ModuleDefMD module, string assemblyName, string targetAssemblyPath)
    {
        TypeRef attrTypeRef = module.CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "InternalsVisibleToAttribute");
        var ctorSig = MethodSig.CreateInstance(module.CorLibTypes.Void, module.CorLibTypes.String);
        var memberRef = new MemberRefUser(module, ".ctor", ctorSig, attrTypeRef);
        var customAttr = new CustomAttribute(memberRef);
        customAttr.ConstructorArguments.Add(new CAArgument(module.CorLibTypes.String, assemblyName));
        module.Assembly.CustomAttributes.Add(customAttr);
        Log.LogMessage(MessageImportance.Normal, $"Added 'InternalsVisibleToAttribute' for '{assemblyName}' in '{targetAssemblyPath}'");
    }

    private void RemoveSealedFromTypes(ModuleDefMD module, string targetAssemblyPath)
    {
        foreach (TypeDef type in module.Types)
        {
            if (type.IsSealed)
                type.IsSealed = false;
        }

        Log.LogMessage(MessageImportance.Normal, $"Removed 'sealed' from all types in '{targetAssemblyPath}'");
    }

    private static string GetFullFilePath(string basePath, string path) => Path.IsPathRooted(path) ? Path.GetFullPath(path) : Path.Combine(basePath, path);
}