> That seems fundamentally wrong to me
> 
> You cannot use the TFM to pick the UI framework
> 
> It will cause all sorts of problems

> [!CAUTION]
> This project is now abandoned. I do not recommend using it **ever**, as indicated by [Sergio](https://github.com/Sergio0694) (see above) in the Windows App Community Discord server.
> This tooling, created for [CubeKit](https://github.com/RiversideValley/Toolkit), contains major code smells and various problems, including major interference with the .NET SDK and Roslyn analysers.
> The tooling inspired by Alloy can be found in the CubeKit source today, though it unfortunately lacks the feature of having both UWP and WinUI code in one package that was the main selling point of Alloy.
>
> I **do not** recommend using Alloy **in any project**, as, while it is an epic idea, it is not executed very well.
> Thanks for using Alloy!
> 
> PS: the only published version of CubeKit that uses Alloy is [`2.0.0-alpha1`](https://www.nuget.org/packages/Riverside.Toolkit/2.0.0-alpha1) 👀

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

---

> [!TIP]
> Put the following code in a `GlobalUsings.cs` file in your project to make Alloy even easier to use:

```cs
global using global::System;
global using global::System.Collections;
global using global::System.Collections.Generic;
global using global::System.Collections.ObjectModel;
global using global::System.Linq;
global using global::System.Threading;
global using global::System.Threading.Tasks;
global using global::System.ComponentModel;
global using global::System.Diagnostics;
global using global::System.Text.Json;
global using global::System.Text.Json.Serialization;
global using SystemIO = global::System.IO;

#if WinUI
global using global::Microsoft.UI.Xaml;
global using global::Microsoft.UI.Xaml.Controls;
global using global::Microsoft.UI.Xaml.Data;
global using global::Microsoft.UI.Xaml.Input;
global using global::Microsoft.UI.Xaml.Media;
global using global::Microsoft.UI.Xaml.Navigation;
#endif
#if UWP
global using global::Windows.UI.Xaml;
global using global::Windows.UI.Xaml.Controls;
global using global::Windows.UI.Xaml.Data;
global using global::Windows.UI.Xaml.Input;
global using global::Windows.UI.Xaml.Media;
global using global::Windows.UI.Xaml.Navigation;
#endif
```

---

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
