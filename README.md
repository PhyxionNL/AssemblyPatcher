# AssemblyPatcher [![Nuget](https://img.shields.io/nuget/v/AssemblyPatcher)](https://www.nuget.org/packages/AssemblyPatcher/)

An MSBuild task that modifies existing assemblies to add [InternalsVisibleTo](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.internalsvisibletoattribute) attributes, remove `sealed` modifiers, add `virtual` keywords, or change access modifiers to `public`.

## Usage

Add the NuGet package to your project:

```xml
<PackageReference Include="AssemblyPatcher">
   <PrivateAssets>all</PrivateAssets>
   <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

Add an InternalsVisibleTo attribute to `SomeAssembly` with `OwnAssembly` as the assembly name:

```xml
<ItemGroup>
   <AddInternalsVisibleTo Include="SomeAssembly" AssemblyName="OwnAssembly" />
</ItemGroup>
```

Remove `sealed` modifiers from types in `SomeAssembly`:

```xml
<ItemGroup>
   <RemoveSealedFrom Include="SomeAssembly" />
   <!-- or filtered -->
   <RemoveSealedFrom Include="SomeAssembly" TypeNames="SomeAssembly.TypeA;SomeAssembly.Folder.*" />
</ItemGroup>
```

Add `virtual` keywords in `SomeAssembly`:

```xml
<ItemGroup>
   <AddVirtualTo Include="SomeAssembly" />
   <!-- or filtered -->
   <AddVirtualTo Include="SomeAssembly" MemberNames="SomeAssembly.Types::Member*" />
</ItemGroup>
```

Change access modifiers to `public` in `SomeAssembly`:

```xml
<ItemGroup>
   <MakePublic Include="SomeAssembly" />
   <!-- or filtered -->
   <MakePublic Include="SomeAssembly" MemberNames="SomeAssembly.Types::Member*" />
   <MakePublic Include="SomeAssembly" TypeNames="SomeAssembly.TypeA" />
</ItemGroup>
```

## License

MIT
