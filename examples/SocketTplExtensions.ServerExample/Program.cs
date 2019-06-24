using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketTplExtensions.ServerExample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Application started...");

            try
            {
                await StartServer();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fatal exception has been thrown");
                Console.WriteLine(e);
                Console.ResetColor();
            }

            Console.WriteLine("Press <enter> to exit...");
            Console.ReadLine();
        }

        private static async Task StartServer()
        {
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, 11000);

            using (var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    var handler = await listener.AcceptAsync();
                    HandleRequest(handler);
                }
            }
        }

        private static async void HandleRequest(Socket handler)
        {
            Console.WriteLine("Handling a new request");

            using (handler)
            {
                try
                {
                    var buffer = new byte[1024];
                    var sb = new StringBuilder();

                    while (true)
                    {
                        var receivedBytes = await handler.ReceiveAsync(buffer, 0, buffer.Length, SocketFlags.None);
                        if (receivedBytes == 0)
                            continue;

                        var message = Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                        sb.Append(message);
                        if (message.IndexOf("<EOF>", StringComparison.OrdinalIgnoreCase) > -1)
                            break;
                    }

                    var data = sb.ToString();

                    Console.WriteLine("Data: {0}", data);

                    var echoBytes = Encoding.ASCII.GetBytes(data);
                    await handler.SendAsync(echoBytes, 0, echoBytes.Length, SocketFlags.None);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Exception when handling the request");
                    Console.WriteLine(e);
                    Console.ResetColor();
                }

                handler.Shutdown(SocketShutdown.Both);
            }
        }
    }
}
