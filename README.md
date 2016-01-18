# SteamSharp

SteamSharp is a portable class library for use in .NET 4.5+ projects. Due to its portability, this library can be used in Windows Store (8 or higher) and Windows Phone (8 or higher) applications (among others).


## CURRENTLY BROKEN (IMPORTANT NOTE)

Changes have occured to the Steam APIs which break the authentication flow for SteamSharp. As this is not currently an ungoing project, and because it is coded against non-publicly-documented APIs, there's unfortunately no guarantee of an expeditious fix. Please keep that in mind as you are using the library. Other calls with an otherwise authenticated user should continue to be stable.

## Using Unit Test Framework

SteamSharp uses the built in Visual Studio Unit Test Framework. To run, open your Test Explorer from the Test menu --> Windows --> Test Explorer.

## Notable Packages

* Much of the Rest access layer (including portions of the Client, Request, and Response components) is either forked from or inspired by [RestSharp](https://github.com/restsharp/RestSharp)
* SteamSharp uses [NewtonSoft's Json.NET](https://github.com/JamesNK/Newtonsoft.Json) for JSON deserialization
