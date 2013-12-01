# SteamSharp

SteamSharp is a portable class library for use in .NET 4.5+ projects. Due to its portability, this library can be used in Windows Store (8 or higher) and Windows Phone (8 or higher) applications (among others).

## Using Unit Test Framework

SteamSharp uses the uUnit tool for execution and evaluation of the test cases contained in SteamSharp.TestFramework.

Quick set up:

* Add the xUnit reference to `SteamSharp.TestFramework` in Visual Studio (NuGet is an easy way of accomplishing this)
* Download the xUnit files [here](http://xunit.codeplex.com/releases/view/90058). Be sure to use `xunit.gui.clr4.exe`

Useful resources:

* [How do I use xUnit.net?](http://xunit.codeplex.com/wikipage?title=HowToUse&referringTitle=Home)
* [xUnit FAQ](http://xunit.codeplex.com/)
* [Getting test debugging to work](http://xunit.codeplex.com/discussions/297569)

## Notable Packages

* Much of the Rest access layer (including portions of the Client, Request, and Response components) is either forked from or inspired by [RestSharp](https://github.com/restsharp/RestSharp)
* SteamSharp uses [NewtonSoft's Json.NET](https://github.com/JamesNK/Newtonsoft.Json) for JSON deserialization