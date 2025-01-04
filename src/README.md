# `Alloy`

##### Enhanced custom tooling to bring components to both UWP and WinUI.

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

> Due to the nature of MSBuild, I haven't yet found a way of targeting both UWP and WinUI on .NET 9.
> Currently, you will need to add both TFMs to your project.

Current supported versions include:
- UWP on .NET 9
- WinUI on .NET 8

---

Protip: put the following code in a `GlobalUsings.cs` file in your project to make Alloy even easier to use:
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