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

To add an ```InternalsVisibleToAttribute``` to ```SomeAssembly``` with ```OwnAssembly``` as assembly name, define an ```ItemGroup``` with the following:
```xml
<ItemGroup>
   <AddInternalsVisibleTo Include="SomeAssembly" AssemblyName="OwnAssembly" />
</ItemGroup>
```

To remove sealed modifiers from all types in ```SomeAssembly```, define an ```ItemGroup``` with the following:
```xml
<ItemGroup>
   <RemoveSealed Include="SomeAssembly" />
</ItemGroup>
```

# License
MIT
