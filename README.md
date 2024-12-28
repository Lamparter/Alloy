# `Alloy`

##### The new build system for the Windows Runtime.

_adapted from [the Gist](https://gist.github.com/Lamparter/7747df6950cf2133723ea120fdf1334e)_

---

Alloy is a reimagined way of building apps targeting WinRT: WinAppSDK and UWP.

> Alloy was a stroke of genius I came up with while trying to figure out how on earth I could package both UWP and WindowsAppSDK (WinRT) versions of a class library in one package.
> It's painfully simple (honestly, how has nobody come up with this before?)

When you add Alloy to your project, it will allow you to do:
```cs
#if WinUI
// WinUI implementation
#endif
#if UWP
// UWP implementation
#endif
```

This allows you to create neutral packages that ship with both UWP and WinUI implementations. Since old UWP (with TFM `uap.10.0.x`) is not SDK-style, it is not technically capable of supporting the Alloy build system.

> [!NOTE]
> Due to the nature of MSBuild, I haven't yet found a way of targeting both UWP and WinUI on .NET 9.
> Currently, you will need to add both TFMs to your project.

Current supported versions include:
- UWP on .NET 9
- WinUI on .NET 8

## What does an Alloy project look like?

> [!CAUTION]
> I'm still figuring some of these things out, and I'm trying to understand how to distribute Alloy - the distribution method will change how this works entirely.

The default template for UWP and WinUI projects are as follows:

**UWP .NET 9**

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0-windows10.0.26100.0</TargetFramework>
    <TargetPlatformMinVersion>10.0.17763.0</TargetPlatformMinVersion>
    <UseUwp>true</UseUwp>
    <UseUwpTools>true</UseUwpTools>
    <IsAotCompatible>true</IsAotCompatible>
    <DisableRuntimeMarshalling>true</DisableRuntimeMarshalling>
  </PropertyGroup>
</Project>
```

**WinUI .NET 9**

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0-windows10.0.19041.0</TargetFramework>
    <TargetPlatformMinVersion>10.0.17763.0</TargetPlatformMinVersion>
    <RuntimeIdentifiers>win-x86;win-x64;win-arm64</RuntimeIdentifiers>
    <UseWinUI>true</UseWinUI>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.Windows.SDK.BuildTools" Version="10.0.26100.1742" />
    <PackageReference Include="Microsoft.WindowsAppSDK" Version="1.6.241114003" />
  </ItemGroup>
</Project>
```

Behind the scenes, an Alloy project looks like this:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0-windows10.0.26100.0</TargetFramework>
    <TargetPlatformMinVersion>10.0.17763.0</TargetPlatformMinVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Alloy)' == 'UWP' ">
    <UseUwp>true</UseUwp>
    <UseUwpTools>true</UseUwpTools>
    <IsAotCompatible>true</IsAotCompatible>
    <DisableRuntimeMarshalling>true</DisableRuntimeMarshalling>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Alloy)' == 'WinUI' ">
    <RuntimeIdentifiers>win-x86;win-x64;win-arm64</RuntimeIdentifiers>
    <UseWinUI>true</UseWinUI>
  </PropertyGroup>
  <ItemGroup Condition=" '$(Alloy)' == 'WinUI' ">
    <PackageReference Include="Microsoft.Windows.SDK.BuildTools" Version="10.0.26100.1742" />
    <PackageReference Include="Microsoft.WindowsAppSDK" Version="1.6.241114003" />
  </ItemGroup>
</Project>
```
