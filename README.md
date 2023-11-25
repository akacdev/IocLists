# IocLists 📝

<div align="center">
  <img width="256" height="256" src="https://raw.githubusercontent.com/actually-akac/IOCLists/master/IocLists/icon.png">
</div>

<div align="center">
  An async and lightweight C# library for interacting with the IOCLists.com API.
</div>

## Usage
This library provides an easy interface for interacting with the [IOC Lists](https://ioclists.com) API.
You can automate your IOC (Indicator of Compromise) management by automatically adding or removing indicators from lists.

To get started, import the library into your solution with either the `NuGet Package Manager` or the `dotnet` CLI.
```rust
dotnet add package IocLists
```

For the primary classes to become available, import the used namespace.
```csharp
using IocLists;
```

An API key is required to interact with the API. Create your own key at: https://ioclists.com/r/settings

Need more examples? Under the `Example` directory you can find a working demo project that implements this library.

## Properties
- Built for **.NET 8**, **.NET 7** and **.NET 6**
- Fully **async**
- Coverage of the current **API**
- Extensive **XML documentation**
- **No external dependencies** (makes use of built-in `HttpClient` and `JsonSerializer`)
- **Custom exceptions** (`IocListsException`) for easy debugging
- Example project to demonstrate all capabilities of the library

## Features
- Create and download IOC lists
- Add and delete entries
- Search for existing indicators submitted by the community

## Code Samples

### Initializing a new API client
```csharp
IocListsClient client = new("d7882f568163547fd5657586ee058f3a7945551745ce36c4bf706741e7907042");
```

### Creating a new list
```csharp
await client.CreateList("akac", "testing-list", "This is a list created using the C# IOC Lists library.");
```

### Appending a new entry to a list
```csharp
await client.Add("akac", "testing-list", "https://example[.]com -- Testing Indicator");
```

### Get 20 recent entries in a list
```csharp
Entry[] recent = await client.GetRecent("mirrors", "phishtank");
```

### Download all unique entries in a list
```csharp
string[] unique = await client.GetUnique("mirrors", "phishtank");
```

### Search for indicators across the entire platform
```csharp
Entry[] matches = await client.Search("62.216.168.7");
```

## Resources
- Website: https://ioclists.com
- Team: https://www.detectdd.com
- API: https://api.ioclists.com/
- Documentation: https://ioclists.com/doc

*This is a community-ran library. Not affiliated with detectdd.*