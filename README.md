# SocketTplExtensions
Extensions methods for `Socket` instances, making it easier to integrate with Task Parallel Library (TPL). It converts all available `BeginX` and `EndX` methods into `XAsync` overloads, covering some missing from .NET Standard, but specially useful in older versions of the framework were none where provided (ex: .NET 4.0).

## Installation 
This library can be installed via [NuGet](https://www.nuget.org/packages/SocketTplExtensions/) package. Just run the following command:

```powershell
Install-Package SocketTplExtensions
```

## Compatibility

This library is compatible with the folowing frameworks:

* .NET Framework 4.0
* .NET Framework 4.5
* .NET Standard 1.3
* .NET Standard 2.0

## Usage
```csharp
using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
{
  await socket.ConnectAsync("127.0.0.1", 12345);
  
  var messageBytes = Encoding.ASCII.GetBytes("Hello world!<EOF>");
  await socket.SendAsync(messageBytes, 0, messageBytes.Length, SocketFlags.None);
  
  var responseBuffer = new byte[1024];
  var responseBuilder = new StringBuilder();
  
  while(true)
  {
    var receivedBytes = await socket.ReceiveAsync(responseBuffer, 0, responseBuffer.Length, SocketFlags.None);
    if (receivedBytes == 0)
      continue;
      
    var response = Encoding.ASCII.GetString(responseBuffer, 0, receivedBytes);
    sb.Append(message);
    if (message.IndexOf("<EOF>", StringComparison.OrdinalIgnoreCase) > -1)
      break;
  }
  
  await socket.DisconnectAsync(false);
}
```
