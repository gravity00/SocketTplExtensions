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

## Extensions

List of extension methods provided:

Method | .NET 4.0 | .NET 4.5 | .NET Standard 1.3 | .NET Standard 2.0 |
 -------- | --------- | --------- | --------------------| --------------------|
AcceptAsync() | A | A | F | F |
ConnectAsync(host, port) | A | A | F | F |
ConnectAsync(address, port) | A | A | F | F |
ConnectAsync(addresses, port) | A | A | F | F |
ConnectAsync(endpoint) | A | A | F | F |
DisconnectAsync(reuseSocket) | A | A | NS | A |
ReceiveAsync(buffer, offset, size, socketFlags) | A | A | A | A |
ReceiveAsync(buffers, socketFlags) | A | A | F | F |
SendAsync(buffer, offset, size, socketFlags) | A | A | A | A |
SendAsync(buffers, socketFlags) | A | A | F | F |
SendFileAsync(filename) | A | A | NS | A |
SendMessageAsync(message, encoding, socketFlags) | A | A | A | A |
SendMessageAsync(message, encoding, buffer, bufferIndex, socketFlags) | A | A | A | A |
SendMessageAsync(chars, index, count, encoding, socketFlags) | A | A | A | A |
SendMessageAsync(chars, index, count, encoding, buffer, bufferIndex, socketFlags) | A | A | A | A |
SendToAsync(buffer, offset, size, socketFlags, endpoint) | A | A | A | A |

Legend:
* _A_ - available;
* _F_ - provided by the framework;
* _NS_ - not supported;

## Usage
```csharp
using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
{
  await socket.ConnectAsync("127.0.0.1", 12345);
  
  await socket.SendMessageAsync("Hello world!<EOF>", Encoding.ASCII, SocketFlags.None);
  
  var responseBuffer = new byte[1024];
  var responseBuilder = new StringBuilder();
  
  while(true)
  {
    var receivedBytes = await socket.ReceiveAsync(responseBuffer, 0, responseBuffer.Length, SocketFlags.None);
    if (receivedBytes == 0)
      continue;
      
    var response = Encoding.ASCII.GetString(responseBuffer, 0, receivedBytes);
    sb.Append(response);
    if (response.IndexOf("<EOF>", StringComparison.OrdinalIgnoreCase) > -1)
      break;
  }
  
  await socket.DisconnectAsync(false);
}
```
