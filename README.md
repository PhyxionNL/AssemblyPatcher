# AssemblyPatcher

An MSBuild task that adds [InternalsVisibleTo](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.internalsvisibletoattribute) attributes or removes sealed modifiers from existing assemblies.

# Usage
First add the [NuGet package](https://www.nuget.org/packages/AssemblyPatcher) to your project:
```xml
<PackageReference Include="AssemblyPatcher">
   <PrivateAssets>all</PrivateAssets>
   <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

Then define an ItemGroup with the following:
```xml
<ItemGroup>
   <AddInternalsVisibleTo Include="SomeAssembly" AssemblyName="OwnAssembly" />
</ItemGroup>
```

This will add an InternalsVisibleToAttribute to SomeAssembly with OwnAssembly as assembly name.

You can also remove sealed modifiers from assembly types like this:
```xml
<ItemGroup>
   <RemoveSealed Include="SomeAssembly" />
</ItemGroup>
```

# License
MIT
