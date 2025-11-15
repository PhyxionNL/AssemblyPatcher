# InternalsVisibleToPatcher

An MSBuild task to add [InternalsVisibleTo](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.internalsvisibletoattribute) attributes to existing assemblies.

# Usage
First add the [NuGet package](https://www.nuget.org/packages/InternalsVisibleToPatcher) to your project:
```xml
<PackageReference Include="InternalsVisibleToPatcher">
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

This will add an InternalsVisibleToAttribute to SomeAssembly with "OwnAssembly" as assembly name.

# License
MIT
